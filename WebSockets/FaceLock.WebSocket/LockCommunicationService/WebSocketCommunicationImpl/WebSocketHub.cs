using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FaceLock.WebSocket.LockCommunicationService.WebSocketCommunicationImpl
{
    public class WebSocketHub
    {
        private static readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _locksSerialNumber = new();
        private static readonly ConcurrentDictionary<string, HttpContext> _locksWebSocketContexts = new();
        private static List<string> whiteListOfDoorLocks = new();

        private readonly ILogger<WebSocketHub> _logger;

        public WebSocketHub(ILogger<WebSocketHub> logger)
        {
            _logger = logger;
        }


        public static void AddToWhiteList(string serialNumber)
        {
            if(serialNumber  == null)
            {
                return;
            }

            whiteListOfDoorLocks.Add(serialNumber);
        }

        public async Task StartAsync(HttpContext ctx)
        {
            Console.WriteLine($"WebSocket server started on {ctx.Request.Host}:{ctx}");

            var newWebsocket = await ctx.WebSockets.AcceptWebSocketAsync();
            
            var ipAdressClient = $"{ctx.Connection.RemoteIpAddress}:{ctx.Connection.RemotePort}";

            string serialNumber = ctx.Request.Headers["serialNumber"];

            if(!whiteListOfDoorLocks.Contains(serialNumber) || serialNumber == null)
            {
                await newWebsocket?.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
                newWebsocket.Dispose();
                return;
            }

            _locksSerialNumber.TryAdd(serialNumber, newWebsocket);
            _locksWebSocketContexts.TryAdd(serialNumber, ctx);

            Console.WriteLine($"{ipAdressClient} connected");

            await HandleLockAsync(serialNumber, newWebsocket);
        }

        private async Task HandleLockAsync(string lockId, System.Net.WebSockets.WebSocket webSocket)
        {
            var buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);

                        Console.WriteLine($"Received message from lock {lockId}: {message}");
                        _logger.LogInformation($"Received message from lock {lockId}: {message}");

                        var responseMessage = "Message received and processed.";
                        await SendAsync(webSocket, responseMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling lock {lockId}: {ex.Message}");
                    _logger.LogError($"Error handling lock {lockId}: {ex.Message}");
                    break;
                }
            }

            _locksSerialNumber.TryRemove(lockId, out _);
            _locksWebSocketContexts.TryRemove(lockId, out _);
            webSocket.Dispose();
            Console.WriteLine($"Lock {lockId} disconnected.");
        }

        public async Task SendAsync(string lockId, string message)
        {
            if (_locksSerialNumber.TryGetValue(lockId, out var webSocket))
            {
                await SendAsync(webSocket, message);
            }
            else
            {
                Console.WriteLine($"Lock {lockId} not found.");
            }
        }

        public async Task SendAsync(string lockId, string message, string jwtToken)
        {
            if (_locksSerialNumber.TryGetValue(lockId, out var webSocket))
            {
                await SendAsync(webSocket, $"{message}{jwtToken}");
            }
            else
            {
                Console.WriteLine($"Lock {lockId} not found.");
            }
        }

        private async Task SendAsync(System.Net.WebSockets.WebSocket webSocket, string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(data);

            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

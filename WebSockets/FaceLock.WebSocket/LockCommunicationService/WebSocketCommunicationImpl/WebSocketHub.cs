using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FaceLock.WebSocket.LockCommunicationService.WebSocketCommunicationImpl
{
    public class SerialNumberDoorLock
    {
        public string SerialNumber { get; set; }
    }

    public class WebSocketHub
    {
        private static readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _locksSerialNumber = new();
        private static readonly ConcurrentDictionary<string, HttpContext> _locksWebSocketContexts = new();
        private readonly string _filePath;

        private readonly ILogger<WebSocketHub> _logger;

        public WebSocketHub(ILogger<WebSocketHub> logger, IConfiguration configuration)
        {
            _logger = logger;
            _filePath = configuration["SerialNumbersFilePath"];
        }


        public async Task AddToWhiteListAsync(string serialNumber)
        {
            if(serialNumber  == null)
            {
                return;
            }

            await AddSerialNumberAsync(new SerialNumberDoorLock() { SerialNumber = serialNumber });
        }

        public async Task StartAsync(HttpContext ctx)
        {
            Console.WriteLine($"WebSocket server started on {ctx.Request.Host}:{ctx}");

            var newWebsocket = await ctx.WebSockets.AcceptWebSocketAsync();
            
            var ipAdressClient = $"{ctx.Connection.RemoteIpAddress}:{ctx.Connection.RemotePort}";

            string serialNumber = ctx.Request.Headers["serialNumber"];

            if(!IsExistSerialNumberAsync(serialNumber).Result || serialNumber == null)
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





        public async Task AddSerialNumberAsync(SerialNumberDoorLock serialNumber)
        {
            var serialNumbers = await ReadSerialNumbersFromFileAsync();
            serialNumbers.Add(serialNumber);
            await WriteSerialNumbersToFileAsync(serialNumbers);
        }

        public async Task RemoveSerialNumberAsync(string serialNumber)
        {
            var serialNumbers = await ReadSerialNumbersFromFileAsync();
            serialNumbers.RemoveAll(sn => sn.SerialNumber == serialNumber);
            await WriteSerialNumbersToFileAsync(serialNumbers);
        }

        public async Task<bool> IsExistSerialNumberAsync(string serialNumber)
        {
            var serialNumbers = await ReadSerialNumbersFromFileAsync();
            var serialNumberExist = serialNumbers.Find(sn => sn.SerialNumber == serialNumber);
            return serialNumberExist != null;
        }

        private async Task<List<SerialNumberDoorLock>> ReadSerialNumbersFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<SerialNumberDoorLock>();
            }

            var jsonString = await File.ReadAllTextAsync(_filePath);
            if(jsonString == null || jsonString == "")
            {
                return new List<SerialNumberDoorLock>();
            }
            return JsonSerializer.Deserialize<List<SerialNumberDoorLock>>(jsonString);
        }

        private async Task WriteSerialNumbersToFileAsync(List<SerialNumberDoorLock> tokenStates)
        {
            var jsonString = JsonSerializer.Serialize(tokenStates, new JsonSerializerOptions
            {
                WriteIndented = true // For pretty
            });

            await File.WriteAllTextAsync(_filePath, jsonString);
        }
    }
}

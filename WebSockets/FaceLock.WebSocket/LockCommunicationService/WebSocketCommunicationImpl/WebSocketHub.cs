using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace FaceLock.WebSocket.LockCommunicationService.WebSocketCommunicationImpl
{
    public class WebSocketHub
    {
        private static readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _locksConnections = new();
        private static readonly ConcurrentDictionary<string, HttpContext> _locksWebSocketContexts = new();



        public async Task StartAsync(IPAddress ipAddress, int port)
        {
            var listener = new TcpListener(ipAddress, port);
            listener.Start();

            Console.WriteLine($"WebSocket server started on {ipAddress}:{port}");

            while (true)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();

                // Запускаємо обробку WebSocket-запиту в окремому потоці
                _ = Task.Run(() => ProcessWebSocketRequestAsync(tcpClient));
            }
        }
        private async Task ProcessWebSocketRequestAsync(TcpClient tcpClient)
        {
            HttpListenerWebSocketContext webSocketContext = null;

            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add($"http://{tcpClient.Client.RemoteEndPoint}/");
                listener.Start();

                var context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    webSocketContext = await context.AcceptWebSocketAsync(null);
                    var webSocket = webSocketContext.WebSocket;

                    // Отримуємо унікальний ідентифікатор замка на основі IP-адреси та порту
                    var lockId = tcpClient.Client.RemoteEndPoint.ToString();
                    _locksConnections.TryAdd(lockId, webSocket);

                    Console.WriteLine($"New lock connected: {lockId}");

                    // Обробка повідомлень від замка
                    await HandleLockAsync(lockId, webSocket);
                }
            }
            catch
            {
                webSocketContext?.WebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
            }
        }



        public async Task StartAsync(HttpContext ctx)
        {
            Console.WriteLine($"WebSocket server started on {ctx.Request.Host}:{ctx}");

            var newWebsocket = await ctx.WebSockets.AcceptWebSocketAsync();
            
            var ipAdressClient = $"{ctx.Connection.RemoteIpAddress}:{ctx.Connection.RemotePort}";


            _locksConnections.TryAdd(ipAdressClient, newWebsocket);
            _locksWebSocketContexts.TryAdd(ipAdressClient, ctx);

            Console.WriteLine($"{ipAdressClient} connected");

            await HandleLockAsync(ipAdressClient, newWebsocket);
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

                        // Опрацьовуйте повідомлення від замка тут

                        // Приклад відправки відповіді на замок
                        var responseMessage = "Message received and processed.";
                        await SendAsync(webSocket, responseMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling lock {lockId}: {ex.Message}");
                    break;
                }
            }

            _locksConnections.TryRemove(lockId, out _);
            _locksWebSocketContexts.TryRemove(lockId, out _);
            webSocket.Dispose();
            Console.WriteLine($"Lock {lockId} disconnected.");
        }

        public async Task SendAsync(string lockId, string message)
        {
            if (_locksConnections.TryGetValue(lockId, out var webSocket))
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
            if (_locksConnections.TryGetValue(lockId, out var webSocket))
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

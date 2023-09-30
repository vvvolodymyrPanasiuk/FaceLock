using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace FaceLock.WebSocket.LockCommunicationService.WebSocketCommunicationImpl
{
    public enum WebSocketEvent
    {
        UNKNOWN = -1,
        OPEN_LOCK = 0,
        LOCK_IS_OPEN,
        LOCK_IS_CLOSED
    }

    class OpenLockEvent
    {
        public long expirationTime { get; set; }
    }

    public class WebSocketRequest<T>
    {
        public string eventType { get; set; }
        public T data { get; set; }
    }

    public class WebSocketLockCommunicationStrategy : ILockCommunicationStrategy
    {
        private readonly WebSocketHub _webSocketServer;

        public WebSocketLockCommunicationStrategy(WebSocketHub webSocketServer)
        {
            _webSocketServer = webSocketServer ?? throw new ArgumentNullException(nameof(webSocketServer));
        }

        public async Task<bool> SendToLockAsync(string serialNumber)
        {
            try
            {
                var webSocketEvent = new WebSocketRequest<OpenLockEvent>
                {
                    eventType = WebSocketEvent.OPEN_LOCK.ToString(),
                    data = new OpenLockEvent
                    {
                        expirationTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(10)).ToUnixTimeSeconds()
                    }
                };

                var json = JsonSerializer.Serialize(webSocketEvent);

                await _webSocketServer.SendAsync(serialNumber, json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to lock: {ex.Message}");
                return false;
            }
        }
    }
}

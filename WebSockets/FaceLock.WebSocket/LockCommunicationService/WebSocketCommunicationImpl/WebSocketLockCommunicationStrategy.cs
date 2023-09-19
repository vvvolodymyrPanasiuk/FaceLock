using System.Threading.Tasks;
using System;

namespace FaceLock.WebSocket.LockCommunicationService.WebSocketCommunicationImpl
{
    public class WebSocketLockCommunicationStrategy : ILockCommunicationStrategy
    {
        private readonly WebSocketHub _webSocketServer;

        public WebSocketLockCommunicationStrategy(WebSocketHub webSocketServer)
        {
            _webSocketServer = webSocketServer ?? throw new ArgumentNullException(nameof(webSocketServer));
        }

        public async Task<bool> SendToLockAsync(string url, string message, string jwtToken)
        {
            try
            {
                await _webSocketServer.SendAsync(url, message, jwtToken);

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

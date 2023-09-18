using FaceLock.WebSocket.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FaceLock.WebSocket.Services
{
    public class DoorLockService : DoorLock.DoorLockBase
    {
        private readonly ILogger<DoorLockService> _logger;
        public DoorLockService(ILogger<DoorLockService> logger) 
        {
            _logger = logger;
        }


        public override Task<DoorLockServiceResponse> OpenDoorLock(DoorLockServiceRequest request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is null"), "Bad request");
                }
                var token = request.Token;
                var url = request.Url;

                // Logic to open the door lock
                // ...

                _logger.LogInformation($"200: Door lock: OPEN");
                return Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 200,
                    Message = "Door lock: OPEN"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 500,
                    Message = "Error: " + ex.Message
                });
            }        
        }      
    }
}

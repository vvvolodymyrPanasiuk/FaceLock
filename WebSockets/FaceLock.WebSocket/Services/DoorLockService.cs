using FaceLock.WebSocket.Protos;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace FaceLock.WebSocket.Services
{
    public class DoorLockService : DoorLock.DoorLockBase
    {
        public DoorLockService() { }

        public override Task<DoorLockServiceResponse> OpenDoorLock(DoorLockServiceRequest request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is null"), "Bad request");
                }
                var token = request.Token;

                // Logic to open the door lock
                // ...

                return Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 200,
                    Message = "Door lock: OPEN"
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new DoorLockServiceResponse
                {
                    Status = 500,
                    Message = "Error: " + ex.Message
                });
            }        
        }      
    }
}

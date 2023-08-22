using FaceLock.WebAPI.Clients.GrpcClient.GrpcClientChannelFactory;
using FaceLock.WebSocket.Protos;
using System.Threading.Tasks;

namespace FaceLock.WebAPI.Clients.GrpcClient.GrpcClientImplementation
{
    public class GrpcDoorLockClient : IGrpcDoorLockClient
    {
        private readonly IGrpcClientChannelFactory _grpcClientChannelFactory;

        public GrpcDoorLockClient(IGrpcClientChannelFactory grpcClientChannelFactory)
        {
            _grpcClientChannelFactory = grpcClientChannelFactory;
        }

        public async Task<DoorLockServiceResponse> OpenDoorLockAsync(string token)
        {
            using (var channel = _grpcClientChannelFactory.CreateGrpcClientChannel())
            {
                var client = new DoorLock.DoorLockClient(channel);
                var request = new DoorLockServiceRequest
                {
                    Token = token
                };

                var response = await client.OpenDoorLockAsync(request);
                return response;
            }
        }
    }
}

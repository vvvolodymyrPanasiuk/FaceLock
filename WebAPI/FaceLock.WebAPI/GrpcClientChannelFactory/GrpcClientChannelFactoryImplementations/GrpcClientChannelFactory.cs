using Grpc.Net.Client;

namespace FaceLock.WebAPI.GrpcClientFactory.GrpcClientFactoryImplementations
{
    public class GrpcClientChannelFactory : IGrpcClientChannelFactory
    {
        private readonly string _grpcServerAddress;
        public GrpcClientChannelFactory(string grpcServerAddress)
        {
            _grpcServerAddress = grpcServerAddress;
        }

        public GrpcChannel CreateGrpcClientChannel()
        {
            return GrpcChannel.ForAddress(_grpcServerAddress);
        }
    }
}
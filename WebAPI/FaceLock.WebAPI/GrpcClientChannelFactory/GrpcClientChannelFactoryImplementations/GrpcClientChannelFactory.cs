using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;

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
            return GrpcChannel.ForAddress(_grpcServerAddress,
                        new GrpcChannelOptions()
                        {
                            HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler())
                            {
                                HttpVersion = System.Net.HttpVersion.Version20
                            }
                        });
        }
    }
}
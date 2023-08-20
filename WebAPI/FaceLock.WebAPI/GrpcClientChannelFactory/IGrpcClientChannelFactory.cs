using Grpc.Net.Client;

namespace FaceLock.WebAPI.GrpcClientFactory
{
    /// <summary>
    /// Interface for a factory to create gRPC client channels.
    /// </summary>
    public interface IGrpcClientChannelFactory
    {
        /// <summary>
        /// Creates a gRPC client channel.
        /// </summary>
        /// <returns>A GrpcChannel instance for communication with the gRPC server.</returns>
        GrpcChannel CreateGrpcClientChannel();
    }
}
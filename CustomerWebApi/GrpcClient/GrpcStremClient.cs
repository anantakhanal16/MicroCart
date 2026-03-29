using Grpc.Core;
using StreamDemo.Grpc;

namespace CustomerWebApi.GrpcClient
{
    public interface IGrpcStremClient
    {
        Task<List<string>> SendStreamAsync(int count);
    }
    public class GrpcStremClient: IGrpcStremClient
    {
        private readonly StreamDemoGrpc.StreamDemoGrpcClient _demoGrpcClient;

        public GrpcStremClient(StreamDemoGrpc.StreamDemoGrpcClient demoGrpcClient)
        {
            _demoGrpcClient = demoGrpcClient;
        }

        public async Task<List<string>> SendStreamAsync(int count)
        {
            var results = new List<string>();

            // Make the streaming call
            var call = _demoGrpcClient.StreamData(new StreamDataRequest
            {
                Count = count
            });

            // Read all streamed responses from server
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Received from server: {response.Number} - {response.Message}");
                results.Add(response.Message);
            }

            return results;
        }
    }
}
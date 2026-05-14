using Grpc.Core;
using StreamDemo.Grpc;

namespace OrderApi.Infrastructure.Services.GrpcServices.ServerSideStreaming
{
    public class GrpcStreamDemoService : StreamDemoGrpc.StreamDemoGrpcBase
    {
        public override async Task StreamData(StreamDataRequest request, IServerStreamWriter<StreamDataReply> responseStream, ServerCallContext context)
        {
            var random = new Random();

            for (int i = 0; i <= 20; i++)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var data = random.Next(1, 101);

                var reply = new StreamDataReply
                {
                    Number=  i ,
                    Message = $"Random value {i}: {data}"
                };

                await responseStream.WriteAsync(reply);

                await Task.Delay(500);
            }
        }
    }
}

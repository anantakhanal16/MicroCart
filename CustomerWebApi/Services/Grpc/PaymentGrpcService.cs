using System.Threading.Tasks;
using CustomerWebApi.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CustomerWebApi.Services.Grpc
{
    public class PaymentGrpcService : PaymentGrpc.PaymentGrpcBase
    {
        private readonly ILogger<PaymentGrpcService> _logger;

        public PaymentGrpcService(ILogger<PaymentGrpcService> logger)
        {
            _logger = logger;
        }

        public override Task<PaymentReply> ProcessPayment(
            PaymentRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation(
                "Processing payment for OrderId: {OrderId}, Amount: {Amount}",
                request.OrderId,
                request.Amount);

            var reply = new PaymentReply
            {
                Status = request.Amount > 0 ? "Success" : "Failed"
            };

            return Task.FromResult(reply);
        }
    }
}
using CustomerWebApi.Grpc;
using Grpc.Core;

namespace OrderApi.Services.GrpcServices
{
    public class PaymentGrpcService : PaymentGrpc.PaymentGrpcBase
    {
        private readonly ILogger<PaymentGrpcService> _logger;

        public PaymentGrpcService(ILogger<PaymentGrpcService> logger)
        {
            _logger = logger;
        }

        public override Task<PaymentReply> ProcessPayment(PaymentRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Processing payment for OrderId: {OrderId}, Amount: {Amount}", request.OrderId, request.Amount);

            var paymentSuccessful = true; 

            var status = paymentSuccessful ? "Success" : "Failed";

            var reply = new PaymentReply
            {
                Status = status
            };

            return Task.FromResult(reply);
        }
    }
}

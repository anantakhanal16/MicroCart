using CustomerWebApi.Grpc;

namespace CustomerWebApi.GrpcClient
{
    public class PaymentService
    {
        private readonly PaymentGrpc.PaymentGrpcClient _client;

        public PaymentService(PaymentGrpc.PaymentGrpcClient client)
        {
            _client = client;
        }

        public async Task<string> ProcessPaymentAsync(string orderId, double amount)
        {
            var request = new PaymentRequest
            {
                OrderId = orderId,
                Amount = amount
            };

            var response = await _client.ProcessPaymentAsync(request);

            return response.Status;
        }
    }
}


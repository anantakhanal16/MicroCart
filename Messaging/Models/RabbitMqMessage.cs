using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Models
{
    public class RabbitMqMessage<T>
    {
        public T Message { get; set; }
        public string Exchange { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        public RabbitMqMessage(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
        {
            Message = message;
            Exchange = exchange;
            RoutingKey = routingKey;
            CancellationToken = cancellationToken;
        }
    }
}

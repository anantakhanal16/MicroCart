using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Models;

namespace Messaging.Interfaces
{
    public interface IRabbitMqService
    {
      //  Task Publish<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken);
        Task Publish<T>(RabbitMqMessage<T> message,CancellationToken cancellationToken);
    }
}

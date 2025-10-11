using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContracts
{
    public class OrderCreatedEvent
    {
        public string ProductName { get; set; } // assuming product name is here
        public string CustomerName { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public string Id { get; set; }
    }
}

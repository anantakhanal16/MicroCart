namespace OrderApi.Application.Dto
{
    public class OrderDto
    {
        public string OrderId { get; set; }
        public int CustomerId { get; set; }
        public string OrderedOn { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}

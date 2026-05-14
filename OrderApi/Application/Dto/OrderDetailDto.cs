namespace OrderApi.Application.Dto
{
    public class OrderDetailDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

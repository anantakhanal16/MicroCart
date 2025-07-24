using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models
{
    [Table("product")]
    public class Products
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_code { get; set; }
        public decimal product_price { get; set; }
    }
}

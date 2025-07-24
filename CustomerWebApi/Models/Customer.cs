using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerWebApi.Models
{
    [Table("customer", Schema ="dbo")]
    public class Customer
    {
        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Data
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public decimal Sum { get; set; }

        public decimal UnitPrice { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Supermarket")]
        public int SupermarketId { get; set; }

        public virtual Supermarket Supermarket { get; set; }
    }
}

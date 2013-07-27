using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int MeasureId { get; set; }

        public virtual Measure Measure { get; set; }

        public Product()
        {
            this.orders = new HashSet<Order>();
        }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }

        private ICollection<Order> orders;

        public virtual ICollection<Order> Orders
        {
            get { return this.orders; }
            set { this.orders = value; }
        }
    }
}

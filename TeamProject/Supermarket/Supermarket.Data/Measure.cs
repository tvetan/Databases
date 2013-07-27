using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Data
{
    public class Measure
    {
        [Key]
        public int MeasureId { get; set; }

        public string Name { get; set; }

        public Measure()
        {
            this.products = new HashSet<Product>();
        }

        private ICollection<Product> products;

        public virtual ICollection<Product> Products
        {
            get { return this.products; }
            set { this.products = value; }
        }
        
    }
}

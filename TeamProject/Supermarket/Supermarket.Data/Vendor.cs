using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Data
{
    public class Vendor
    {
        [Key]
        public int VendorId { get; set; }

        public string Name { get; set; }

        public Vendor()
        {
            this.products = new HashSet<Product>();
        }

        private ICollection<Product> products;

        public virtual ICollection<Product> Products
        {
            get { return this.products; }
            set { this.products = value; }
        }

        private ICollection<Expense> expenses;
        
        public virtual ICollection<Expense> Expenses
        {
            get { return this.expenses; }
            set { this.expenses = value; }
        }
    }
}

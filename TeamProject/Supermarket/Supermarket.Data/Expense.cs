using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarkets.Data
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        
        public string Month { get; set; }

        public decimal ExpenseValue { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}

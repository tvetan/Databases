using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlExpenses
{
    public class ExpenseValue
    {
        public string Month { get; set; }
        public decimal Value { get; set; }
    }

    public class ExpenseDocument
    {
        public int ExpenseDocId { get; set; }
        public string ExpenseVendor { get; set; }

        public ExpenseValue[] Expenses { get; set; }
    }
}

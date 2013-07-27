using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDFGenerator
{
    class DateReport
    {
        private const int cols = 5;

        public DateReport()
        {
            this.Cells = new List<PdfPCell>();
            this.Sums = new List<decimal>();
        }

        public DateTime DateTime { get; set; }

        public List<PdfPCell> Cells { get; set; }

        public List<decimal> Sums { get; set; }            

        public decimal CalculateTotalSum()
        {
            decimal result = 0m;

            foreach (var sum in this.Sums)
            {
                result += sum;
            }

            return result;
        }
    }
}

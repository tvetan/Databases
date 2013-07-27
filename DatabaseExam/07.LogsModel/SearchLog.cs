using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07.LogsModel
{
    public class SearchLog
    {
      //  Id	Date	QueryXml
        public int SearchLogId { get; set; }

        public DateTime Date { get; set; }

        public string QueryXml { get; set; }      
    }
}

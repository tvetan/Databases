using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _07.LogsModel;

namespace _07.LogsData
{
    public class LogsContext : DbContext
    {
        public LogsContext()
            : base("Logs")
        {
        }


        public DbSet<SearchLog> Logs { get; set; }
    }
}

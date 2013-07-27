using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _07.LogsData;
using _07.LogsModel;
using _07.LogsData.Migrations;

namespace _07.LogsClient
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<LogsContext, Configuration>());

            LogsContext logsContext = new LogsContext();
            using (logsContext)
            {
                SearchLog log = new SearchLog()
                {
                    Date = DateTime.Now,
                    QueryXml = "some xml"
                };
                logsContext.Logs.Add(log);
                logsContext.SaveChanges();
                //foreach (var item in logsContext.Logs)
                //{
                //    Console.WriteLine(item.QueryXml);
                //}
            }
        }
    }
}
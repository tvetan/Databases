using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SuperMarket.Model;
using Supermarkets.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace XmlExpenses
{
    public class XmlImporter
    {
        public static string vendorXmlFilePath = "../../../imports/Vendors-Expenses.xml";
        private static Dictionary<string, List<ExpenseValue>> inputExpenses = new Dictionary<string, List<ExpenseValue>>();
        
        static private void ImportFromXml()
        {
            using (XmlReader reader = XmlReader.Create(vendorXmlFilePath))
            {
                string currVendorName = string.Empty;
                string currExpenseMonth = string.Empty;

                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "sale"))
                    {
                        currVendorName = reader.GetAttribute("vendor");
                        if (!inputExpenses.ContainsKey(currVendorName))
                        {
                            inputExpenses.Add(currVendorName, new List<ExpenseValue>());
                        }
                    }

                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "expenses"))
	                {
                        currExpenseMonth = reader.GetAttribute("month");
	                }

                    if ((reader.NodeType == XmlNodeType.Text))
                    {
                        ExpenseValue expense = new ExpenseValue();
                        expense.Month = currExpenseMonth;
                        expense.Value = Decimal.Parse(reader.Value);

                        inputExpenses[currVendorName].Add(expense);
                    }
                }
            }
        }

        static public void ImportXmlToSql()
        {
            ImportFromXml();

            SuperMarketContext db = new SuperMarketContext();

            using (db)
            {   
                foreach (var expenseVendor in inputExpenses)
                {
                    Vendor expVendor = db.Vendors.FirstOrDefault(v => v.Name == expenseVendor.Key);

                    if (expVendor != null)
	                {
		                foreach (var expValue in expenseVendor.Value)
	                    {
                            Expense newExpense = new Expense { Month = expValue.Month, ExpenseValue = expValue.Value };
                            db.Expenses.Add(newExpense);
                            expVendor.Expenses.Add(newExpense);

                            db.SaveChanges();
	                    }   
	                }
                }
            }
        }

        static public void ImportFromXmlToMongo()
        {
            ImportFromXml();

            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();

            var database = mongoServer.GetDatabase("ProductsDb");
            var collection = database.GetCollection<Expense>("Expenses");

            foreach (var expVendor in inputExpenses)
            {
                var expenseDoc = new ExpenseDocument { ExpenseVendor = expVendor.Key };
                
                int expValuesCount = expVendor.Value.Count;
                expenseDoc.Expenses = new ExpenseValue[expValuesCount];

                int counter = 0;
                foreach (var expValue in expVendor.Value)
                {
                    expenseDoc.Expenses[counter] = new ExpenseValue { Month = expValue.Month, Value = expValue.Value };
                }

                collection.Insert(expenseDoc);
            }
        }
    }
}

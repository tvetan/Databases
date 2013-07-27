namespace SuperMarket.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Supermarkets.Data;
    using Telerik.OpenAccess;
    using System.Transactions;
    using System.Collections.Generic;
    using ProductsData;

    public sealed class Configuration : DbMigrationsConfiguration<SuperMarketContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SuperMarketContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var oaDb = new ProductsData.Model.ProductsData();
            IEnumerable<ProductsData.Model.Product> productsData;
            using (oaDb)
            {
                var productsDataQuery = oaDb.Products;
                productsData = productsDataQuery.ToList();
            }
            // get the context and start
            var sqlDb = new SuperMarketContext();
            if (sqlDb.Products.ToList().Count > 0)
            {
                sqlDb.Dispose();
            }
            else
            {
                using (sqlDb)
                {
                    int stop = 0;
                    foreach (var productData in productsData)
                    {
                        Product product = sqlDb.Products.FirstOrDefault(p => p.ProductId == productData.ProductsId);
                        if (product == null)
                        {
                            product = new Product { Name = productData.ProductName };
                        }
                        sqlDb.Products.AddOrUpdate(product);
                        // Add to measures
                        Measure measure = sqlDb.Measures.FirstOrDefault(m => m.Name == productData.Measure.MeasureName);
                        if (measure == null)
                        {
                            measure = new Measure { Name = productData.Measure.MeasureName };
                        }
                        measure.Products.Add(product);
                        sqlDb.Measures.AddOrUpdate(measure);
                        Vendor vendor = sqlDb.Vendors.FirstOrDefault(v => v.Name == productData.Vendor.VendorName);
                        if (vendor == null)
                        {
                            vendor = new Vendor { Name = productData.Vendor.VendorName };
                        }
                        vendor.Products.Add(product);
                        sqlDb.Vendors.AddOrUpdate(vendor);
                        sqlDb.SaveChanges();
                        stop++;
                    }
                }
            }
        }
    }
}

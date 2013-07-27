using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarkets.Data;

namespace SuperMarket.Model
{
    public class SuperMarketContext : DbContext
    {
        public SuperMarketContext()
            : base("SupermarketDb")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Configurations.Add(new StudentMappings());
        //    modelBuilder.Configurations.Add(new CourseMappings());
        //    modelBuilder.Configurations.Add(new HomeworkMappings());

        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Product> Products { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Supermarket> Supermarkets { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Measure> Measures { get; set; }

        public DbSet<Expense> Expenses { get; set; }
    }
}

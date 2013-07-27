﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Supermarkets.Data
{
    public class Supermarket
    {
        [Key]
        public int SupermarketId { get; set; }

        public string Name { get; set; }

        public Supermarket()
        {
            this.orders = new HashSet<Order>();
        }

        private ICollection<Order> orders;

        public virtual ICollection<Order> Orders
        {
            get { return this.orders; }
            set { this.orders = value; }
        }
    }
}
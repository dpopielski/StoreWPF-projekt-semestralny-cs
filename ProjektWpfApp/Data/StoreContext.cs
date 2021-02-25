using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektWpfApp
{
    public class StoreContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public void Load()
        {
            Customers.Load();
            Manufacturers.Load();
            Products.Load();
            Orders.Load();
        }
    }

    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(12)]
        public string Phone { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
    }

    public class Manufacturer
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(10)]
        public string NIP { get; set; }
    }

    public class Order
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }

    public class Product
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}

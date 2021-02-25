using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektWpfApp
{
    /// <summary>
    /// Class for manipulating data in a db
    /// </summary>
    public class StoreData
    {
        StoreContext Context;
        public StoreData()
        {
            Context = new StoreContext();
            Context.Load();
        }
        public DbSet<Customer> Customers => Context.Customers;
        public DbSet<Manufacturer> Manufacturers => Context.Manufacturers;
        public DbSet<Product> Products => Context.Products;
        public DbSet<Order> Orders => Context.Orders;

        #region Customer
        public void AddCustomer(string name, string lastname, string phone, string address)
        {
            CheckCustomer(name, lastname, phone, address);
            if (Customers.Where(c => c.Phone == phone).Count() != 0) throw new Exception("There is already a customer with this phone number.");

            Customers.Add(new Customer()
            {
                Name = name,
                LastName = lastname,
                Phone = phone,
                Address = address
            });
            Context.SaveChanges();
        }
        public void EditCustomer(int id, string name, string lastname, string phone, string address)
        {
            CheckCustomer(name, lastname, phone, address);
            var toEdit = Customers.Find(id);
            toEdit.Name = name;
            toEdit.LastName = lastname;
            toEdit.Phone = phone;
            toEdit.Address = address;
            Context.Entry(toEdit).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void RemoveCustomer(int id)
        {
            var customer = Customers.Find(id);
            if (Orders.Where(o => o.Customer.ID == id).Count() != 0) throw new Exception("You can't remove a customer with pending orders.");

            Customers.Remove(customer);
            Context.SaveChanges();
        }
        private void CheckCustomer(string name, string lastname, string phone, string address)
        {
            Check(name, "Name", 30);
            Check(lastname, "Last Name", 30);
            Check(phone, "Phone", 12);
            Check(address, "Address", 100);
        }
        #endregion

        #region Manufacturer
        public void AddManufacturer(string name, string address, string nip)
        {
            CheckManufacturer(name, address, nip);
            if (Manufacturers.Where(m => m.NIP == nip).Count() != 0) throw new Exception("There is already a manufacturer with this NIP number.");

            Manufacturers.Add(new Manufacturer()
            {
                Name = name,
                Address = address,
                NIP = nip
            });
            Context.SaveChanges();
        }
        public void EditManufacturer(int id, string name, string address, string nip)
        {
            CheckManufacturer(name, address, nip);
            var toEdit = Manufacturers.Find(id);
            toEdit.Name = name;
            toEdit.Address = address;
            toEdit.NIP = nip;
            Context.Entry(toEdit).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void RemoveManufacturer(int id)
        {
            if (Products.Where(o => o.Manufacturer.ID == id).Count() != 0) throw new Exception("You can't remove a manufacturer that some products reference.");
            Manufacturers.Remove(Manufacturers.Find(id));
            Context.SaveChanges();
        }
        private void CheckManufacturer(string name, string address, string nip)
        {
            Check(name, "Name", 50);
            Check(address, "Address", 100);
            Check(nip, "NIP", 10);
        }
        #endregion

        #region Product
        public void AddProduct(string name, decimal price, int amount, Manufacturer manufacturer)
        {
            CheckProduct(name);
            if (Products.Where(p => p.Name == name).Count() != 0) throw new Exception("There is already a product with this name.");

            Products.Add(new Product()
            {
                Name = name,
                Price = price,
                Amount = amount,
                Manufacturer = manufacturer
            });
            Context.SaveChanges();
        }
        public void EditProduct(int id, string name, decimal price, int amount, Manufacturer manufacturer)
        {
            CheckProduct(name);
            var toEdit = Products.Find(id);
            toEdit.Name = name;
            toEdit.Price = price;
            toEdit.Amount = amount;
            toEdit.Manufacturer = manufacturer;
            Context.Entry(toEdit).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void RemoveProduct(int id)
        {
            Products.Remove(Products.Find(id));
            Context.SaveChanges();
        }
        private void CheckProduct(string name)
        {
            Check(name, "Name", 50);
        }
        #endregion

        #region Order
        public void AddOrder(DateTime? date, int amount, Customer customer, Product product)
        {
            CheckOrder(date, customer, product);
            Orders.Add(new Order()
            {
                Date = (DateTime)date,
                Amount = amount,
                Customer = customer,
                Product = product
            });
            Context.SaveChanges();
        }
        public void EditOrder(int id, DateTime? date, int amount, Customer customer, Product product)
        {
            CheckOrder(date, customer, product);
            var toEdit = Orders.Find(id);
            toEdit.Date = (DateTime)date;
            toEdit.Amount = amount;
            toEdit.Customer = customer;
            toEdit.Product = product;
            Context.Entry(toEdit).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void RemoveOrder(int id)
        {
            Orders.Remove(Orders.Find(id));
            Context.SaveChanges();
        }
        private void CheckOrder(DateTime? date, Customer customer, Product product)
        {
            if (date is null) throw new Exception("Date must be set.");
            if (customer is null) throw new Exception("Customer must be set.");
            if (product is null) throw new Exception("Product must be set.");
        }
        #endregion

        private void Check(string check, string name, int length)
        {
            if(check.Length == 0)
            {
                throw new Exception($"{name} length can't be 0.");
            } 
            else if(check.Length > length)
            {
                throw new Exception($"{name} length can't be larger than {length}.");
            }
        }
    }
}

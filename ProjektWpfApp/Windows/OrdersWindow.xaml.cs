using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjektWpfApp
{
    /// <summary>
    /// Logika interakcji dla klasy OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        StoreData storeData;
        int? selectedIndex;
        public OrdersWindow(StoreData sd)
        {
            InitializeComponent();
            clientInput.ItemsSource = sd.Customers.Local.Select(k=>k.Name + " " + k.LastName);
            productInput.ItemsSource = sd.Products.Local.Select(p=>p.Name);
            storeData = sd;
            UpdateGrid();
        }

        private void addButton_Click(object sender, RoutedEventArgs e) => HandleManipulation();
        private void HandleManipulation(bool editing = false)
        {
            if (editing && selectedIndex is null) return;

            if (dateInput.SelectedDate is null && clientInput.SelectedIndex == -1 && productInput.SelectedIndex == -1 && ammInput.Text == "") return;

            int amount;

            try
            {
                if (!int.TryParse(ammInput.Text, out amount)) throw new Exception("Invalid number supplied.");
                Customer customer = storeData.Customers.Where(c => c.Name + " " + c.LastName == ((string)clientInput.SelectedValue ?? "")).FirstOrDefault();
                Product product = storeData.Products.Where(p => p.Name == ((string)productInput.SelectedValue ?? "")).FirstOrDefault();
                if (editing)
                {
                    storeData.EditOrder((int)selectedIndex, dateInput.SelectedDate, amount, customer, product);
                } 
                else
                    storeData.AddOrder(dateInput.SelectedDate, amount, customer, product);

                Clear();
                UpdateGrid();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Clear()
        {
            dataGrid.UnselectAll();
            clientInput.SelectedIndex = productInput.SelectedIndex = -1;
            ammInput.Text = "";
            dateInput.SelectedDate = null;
            selectedIndex = null;
        }
        private void UpdateGrid()
        {
            List<OrderEntry> items = (
                    from o in storeData.Orders
                    join c in storeData.Customers on o.Customer equals c
                    join p in storeData.Products on o.Product equals p
                    select new OrderEntry()
                    {
                        ID = o.ID,
                        Date = o.Date,
                        Amount = o.Amount,
                        Customer = c.Name + " " + c.LastName,
                        Product = p.Name
                    }).ToList();
            dataGrid.ItemsSource = items;
        }
        private class OrderEntry
        {
            public int ID { get; set; }
            public DateTime Date { get; set; }
            public int Amount { get; set; }
            public string Customer { get; set; }
            public string Product { get; set; }
        }
        private void clearButton_Click(object sender, RoutedEventArgs e) => Clear();

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1) return;
            var item = (OrderEntry)dataGrid.SelectedItem;
            selectedIndex = item.ID;
            dateInput.SelectedDate = item.Date;
            ammInput.Text = item.Amount.ToString();
            productInput.SelectedItem = item.Product;
            clientInput.SelectedItem = item.Customer;
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex is null) return;
            storeData.RemoveOrder((int)selectedIndex);
            UpdateGrid();
            Clear();
        }

        private void editButton_Click(object sender, RoutedEventArgs e) => HandleManipulation(true);
    }
}

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
    /// Logika interakcji dla klasy AddNewProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        StoreData storeData;
        int? selectedIndex;
        public ProductWindow(StoreData sd)
        {
            InitializeComponent();
            manInput.ItemsSource = sd.Manufacturers.Local.Select(m => m.Name);
            storeData = sd;
            UpdateGrid();
        }
        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addButton_Click(object sender, RoutedEventArgs e) => HandleManipulation();
        private void HandleManipulation(bool editing = false)
        {
            if (editing && selectedIndex is null) return;

            var amm = ammInput.Text;
            var pri = priceInput.Text;
            var name = nameInput.Text;

            if (amm.Length + pri.Length + name.Length == 0 && manInput.SelectedIndex == -1) return;

            int amount;
            decimal price;

            try
            {
                if (!int.TryParse(amm, out amount)) throw new Exception("Invalid number supplied.");
                if (!decimal.TryParse(pri, out price)) throw new Exception("Invalid decimal supplied.");

                Manufacturer manufacturer = storeData.Manufacturers.Where(c => c.Name == ((string)manInput.SelectedValue ?? "")).FirstOrDefault();
                if (editing)
                    storeData.EditProduct((int)selectedIndex, name, price, amount, manufacturer);
                else
                    storeData.AddProduct(name, price, amount, manufacturer);

                Clear();
                UpdateGrid();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Clear()
        {
            dataGrid.UnselectAll();
            ammInput.Text = priceInput.Text = nameInput.Text = "";
            manInput.SelectedIndex = -1;
            selectedIndex = null;
        }
        private void UpdateGrid()
        {
            List<ProductEntry> items = (
                    from p in storeData.Products
                    join m in storeData.Manufacturers
                    on p.Manufacturer equals m
                    select new ProductEntry()
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Manufacturer = m.Name,
                        Amount = p.Amount,
                        Price = p.Price
                    }).ToList();
            dataGrid.ItemsSource = items;
        }
        private class ProductEntry
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Manufacturer { get; set; }
            public int Amount { get; set; }
            public decimal Price { get; set; }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e) => Clear();

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1) return;
            var item = (ProductEntry)dataGrid.SelectedItem;
            selectedIndex = item.ID;
            nameInput.Text = item.Name;
            ammInput.Text = item.Amount.ToString();
            manInput.SelectedItem = item.Manufacturer;
            priceInput.Text = item.Price.ToString();
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex is null) return;
            storeData.RemoveProduct((int)selectedIndex);
            UpdateGrid();
            Clear();
        }

        private void editButton_Click(object sender, RoutedEventArgs e) => HandleManipulation(true);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace ProjektWpfApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainApp.xaml
    /// </summary>
    public partial class StoreWindow : Window
    {
        StoreData storeData;
        public StoreWindow()
        {
            InitializeComponent();
            storeData = new StoreData();
            UpdateGrid();
        }

        private void productsButton_Click(object sender, RoutedEventArgs e) => HandleWindow(new ProductWindow(storeData));
        private void customersButton_Click(object sender, RoutedEventArgs e) => HandleWindow(new CustomerWindow(storeData));
        private void manufButton_Click(object sender, RoutedEventArgs e) => HandleWindow(new ManufacturerWindow(storeData));
        private void ordersButton_Click(object sender, RoutedEventArgs e) => HandleWindow(new OrdersWindow(storeData));
        private void HandleWindow(Window window)
        {
            window.Owner = this;
            try
            {
                window.ShowDialog();
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            UpdateGrid();
        }
        private void UpdateGrid()
        {
            List<MagazineEntry> items = (
                    from p in storeData.Products
                    join m in storeData.Manufacturers
                    on p.Manufacturer equals m
                    select new MagazineEntry()
                    {
                        Name = p.Name,
                        Manufacturer = m.Name+" ["+m.NIP+"]",
                        Ammount = p.Amount,
                        Price = p.Price,
                        Value = p.Price*p.Amount
                    }).ToList();
            dataGrid.ItemsSource = items;
        }
        private class MagazineEntry
        {
            public string Name { get; set; }
            public string Manufacturer { get; set; }
            public int Ammount { get; set; }
            public decimal Price { get; set; }
            public decimal Value { get; set; }
        }
    }
}

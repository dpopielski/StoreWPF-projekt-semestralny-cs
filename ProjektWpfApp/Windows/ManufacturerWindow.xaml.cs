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

namespace ProjektWpfApp
{
    /// <summary>
    /// Logika interakcji dla klasy AddManufacturer.xaml
    /// </summary>
    public partial class ManufacturerWindow : Window
    {
        StoreData storeData;
        int? selectedIndex;
        public ManufacturerWindow(StoreData sd)
        {
            InitializeComponent();
            dataGrid.ItemsSource = sd.Manufacturers.Local;
            storeData = sd;
        }

        private void addButton_Click(object sender, RoutedEventArgs e) => HandleManipulation();
        private void HandleManipulation(bool editing = false)
        {
            if (editing && selectedIndex is null) return;

            var name = nameInput.Text;
            var nip = nipInput.Text;
            var address = addressInput.Text;

            if (name.Length + nip.Length + address.Length == 0) return;

            try
            {
                if (editing)
                {
                    storeData.EditManufacturer((int)selectedIndex, name, address, nip);
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = storeData.Manufacturers.Local;
                }
                else
                    storeData.AddManufacturer(name, address, nip);

                Clear();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Clear()
        {
            dataGrid.UnselectAll();
            nameInput.Text = nipInput.Text = addressInput.Text = "";
            selectedIndex = null;
        }
        private void clearButton_Click(object sender, RoutedEventArgs e) => Clear();

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1) return;
            var item = (Manufacturer)dataGrid.SelectedItem;
            selectedIndex = item.ID;
            nameInput.Text = item.Name;
            addressInput.Text = item.Address;
            nipInput.Text = item.NIP;
        }

        private void editButton_Click(object sender, RoutedEventArgs e) => HandleManipulation(true);
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex is null) return;
            storeData.RemoveManufacturer((int)selectedIndex);
            Clear();
        }
    }
}

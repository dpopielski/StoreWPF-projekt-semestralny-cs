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
    /// Logika interakcji dla klasy AddCustomer.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        StoreData storeData;
        int? selectedIndex;
        public CustomerWindow(StoreData sd)
        {
            InitializeComponent();
            dataGrid.ItemsSource = sd.Customers.Local;
            storeData = sd;
        }

        private void addButton_Click(object sender, RoutedEventArgs e) => HandleManipulation();
        private void Clear()
        {
            dataGrid.UnselectAll();
            nameInput.Text = lnameInput.Text = addressInput.Text = telInput.Text = "";
            selectedIndex = null;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1) return;
            var item = (Customer)dataGrid.SelectedItem;
            selectedIndex = item.ID;
            nameInput.Text = item.Name;
            lnameInput.Text = item.LastName;
            addressInput.Text = item.Address;
            telInput.Text = item.Phone;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex is null) return;
            storeData.RemoveCustomer((int)selectedIndex);
            Clear();
        }

        private void editButton_Click(object sender, RoutedEventArgs e) => HandleManipulation(true);

        private void HandleManipulation(bool editing = false)
        {
            if (editing && selectedIndex is null) return;

            var name = nameInput.Text;
            var lname = lnameInput.Text;
            var address = addressInput.Text;
            var phone = telInput.Text;

            if (name.Length + lname.Length + address.Length + phone.Length == 0) return;

            try
            {
                if (editing)
                {
                    storeData.EditCustomer((int)selectedIndex, name, lname, phone, address);
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = storeData.Customers.Local;
                }
                else
                    storeData.AddCustomer(name, lname, phone, address);
                Clear();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e) => Clear();

    }
}

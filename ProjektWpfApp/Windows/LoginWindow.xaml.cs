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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektWpfApp
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var store = new StoreWindow();

            if (textBoxUsername.Text == "Damian" && textBoxPassword.Password == "Popielski")
            {
                store.Show();
                Close();
            } else
            {
                errormessage.Text = "Proszę wprowadzić poprawną nazwę użytkownika i hasło";
                textBoxUsername.Select(0, textBoxUsername.Text.Length);
                textBoxUsername.Focus();
            }
        }
    }
}

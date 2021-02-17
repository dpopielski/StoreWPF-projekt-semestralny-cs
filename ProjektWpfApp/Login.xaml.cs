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
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            MainApp mainApp = new MainApp();

            if (textBoxUsername.Text.Length == 0)
            {
                errormessage.Text = "Proszę wprowadzić username";
                textBoxUsername.Focus();
            }  else if (!Regex.IsMatch(textBoxUsername.Text, @"[a - z0 - 9]{ 6,20}"))
            {
                errormessage.Text = "Proszę wprowadzić poprawny username";
                textBoxUsername.Select(0, textBoxUsername.Text.Length);
                textBoxUsername.Focus();
            } else
            {
                // View mainApp page
                this.NavigationService.Navigate(mainApp);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    if (textBoxUsername.Text.Length == 0)
        //    {
        //        errormessage.Text = "Proszę wprowadzić username";
        //        textBoxUsername.Focus();
        //    }
        //    else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
        //    {
        //        errormessage.Text = "Enter a valid email.";
        //        textBoxEmail.Select(0, textBoxEmail.Text.Length);
        //        textBoxEmail.Focus();
        //    }
        //    else
        //    {
        //        string email = textBoxEmail.Text;
        //        string password = passwordBox1.Password;
        //        SqlConnection con = new SqlConnection("Data Source=TESTPURU;Initial Catalog=Data;User ID=sa;Password=wintellect");
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("Select * from Registration where Email='" + email + "'  and password='" + password + "'", con);
        //        cmd.CommandType = CommandType.Text;
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = cmd;
        //        DataSet dataSet = new DataSet();
        //        adapter.Fill(dataSet);
        //        if (dataSet.Tables[0].Rows.Count > 0)
        //        {
        //            string username = dataSet.Tables[0].Rows[0]["FirstName"].ToString() + " " + dataSet.Tables[0].Rows[0]["LastName"].ToString();
        //            welcome.TextBlockName.Text = username;//Sending value from one form to another form.  
        //            welcome.Show();
        //            Close();
        //        }
        //        else
        //        {
        //            errormessage.Text = "Sorry! Please enter existing emailid/password.";
        //        }
        //        con.Close();
        //    }
        //}
        //private void buttonRegister_Click(object sender, RoutedEventArgs e)
        //{
        //    registration.Show();
        //    Close();
        //}
    }
}

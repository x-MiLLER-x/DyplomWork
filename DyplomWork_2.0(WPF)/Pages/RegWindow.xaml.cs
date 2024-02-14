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
using System.Windows.Shapes;

namespace DyplomWork_2._0_WPF_
{
    /// <summary>
    /// Interaction logic for RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        DB db;
        public RegWindow()
        {
            InitializeComponent();

            db = new DB();

            //Add PreviewKeyDown event handler
            PreviewKeyDown += Window_PreviewKeyDown;
        }
        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = PassBox.Password.Trim();
            string pass2 = PassBox2.Password.Trim();
            string email = textBoxEmail.Text.Trim().ToLower();

            //Check data
            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "Minimum count of symbols : 5!";
                textBoxLogin.Background = Brushes.DarkRed;
            }
            else if (pass.Length < 5)
            {
                PassBox.ToolTip = "Minimum count of symbols : 5!";
                PassBox.Background = Brushes.DarkRed;
            }
            else if (pass2 != pass)
            {
                PassBox2.ToolTip = "Passwords don't match, please try again.";
                PassBox2.Background = Brushes.DarkRed;
            }
            else if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
            {
                textBoxEmail.ToolTip = "Uncorrectly field";
                textBoxEmail.Background = Brushes.DarkRed;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                PassBox.ToolTip = "";
                PassBox.Background = Brushes.Transparent;
                PassBox2.ToolTip = "";
                PassBox2.Background = Brushes.Transparent;
                textBoxEmail.ToolTip = "";
                textBoxEmail.Background = Brushes.Transparent;

                //Create a new user
                User user = new User(login, pass, email);

                MessageBox.Show("Registration was successful!");

                if (db.try_registration(user))
                {
                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    Close();
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, была ли нажата клавиша Enter
            if (e.Key == Key.Enter)
            {
                // Вызываем метод для авторизации
                Button_Reg_Click(sender, e);
            }
        }

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            Close();
        }

    }
}


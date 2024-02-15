using DyplomWork_2._0_WPF_.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        DB db;
        public AuthWindow()
        {
            InitializeComponent();
            db = new DB();

            // Add PreviewKeyDown event handler
            PreviewKeyDown += Window_PreviewKeyDown;
        }

        // Authorization attempt with button click
        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = PassBox.Password.Trim();

            // Check data validity
            if (login.Length < 5 || pass.Length < 5)
            {
                if (login.Length < 5)
                {
                    textBoxLogin.ToolTip = "Minimum count of symbols : 5!";
                    textBoxLogin.Background = Brushes.DarkRed;
                }
                else
                {
                    textBoxLogin.ToolTip = "";
                    textBoxLogin.Background = Brushes.Transparent;
                }
                if (pass.Length < 5)
                {
                    PassBox.ToolTip = "Minimum count of symbols : 5!";
                    PassBox.Background = Brushes.DarkRed;
                }
                else
                {
                    PassBox.ToolTip = "";
                    PassBox.Background = Brushes.Transparent;
                }
            }
            else
            {
                User authUser = new User();
                authUser.Login = login;
                authUser.Pass = pass;


                if (db.try_autorization(authUser))
                {
                    Pages.MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
            }
        }

        // Authorization attempt by pressing ENTER
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.Key == Key.Enter)
            {
                // Call the method for authorization
                Button_Auth_Click(sender, e);
            }
        }

        // Go to the registration window
        private void Button_Window_Reg_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regWindow = new RegWindow();   
            regWindow.Show();
            Close();
        }

        // Start: Button Close | Restore | Minimize 
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // End: Button Close | Restore | Minimize
    }
}

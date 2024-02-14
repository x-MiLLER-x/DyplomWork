using System;
using System.Collections.Generic;
using System.Linq;
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

            //Add PreviewKeyDown event handler
            PreviewKeyDown += Window_PreviewKeyDown;
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = PassBox.Password.Trim();
            
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
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                PassBox.ToolTip = "";
                PassBox.Background = Brushes.Transparent;

                User authUser = new User();
                authUser.Login = login;
                authUser.Pass = pass;

                if(db.try_autorization(authUser))
                {
                    //UserPageWindow userPageWindow = new UserPageWindow();
                    //userPageWindow.Show();
                    Generate_Meal_Window generateMealWindow = new Generate_Meal_Window();
                    generateMealWindow.Show();
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
                Button_Auth_Click(sender, e);
            }
        }

        private void Button_Window_Reg_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regWindow = new RegWindow();   
            regWindow.Show();
            Close();
        }
    }
}

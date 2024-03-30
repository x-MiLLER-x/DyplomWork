using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

            // Add PreviewKeyDown event handler
            PreviewKeyDown += Window_PreviewKeyDown;
        }
        // Registration attempt with button click
        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = PassBox.Password.Trim();
            string pass2 = PassBox2.Password.Trim();
            string email = textBoxEmail.Text.Trim().ToLower();
            bool comboBoxSavingData = false;

            // Check data validity
            if (login.Length < 5 || pass.Length < 5 || pass2 != pass || email.Length < 5 || !email.Contains("@") || !email.Contains("."))
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
                if (pass2 != pass)
                {
                    PassBox2.ToolTip = "Passwords don't match, please try again.";
                    PassBox2.Background = Brushes.DarkRed;
                }
                else
                {
                    PassBox2.ToolTip = "";
                    PassBox2.Background = Brushes.Transparent;
                }
                if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
                {
                    textBoxEmail.ToolTip = "Uncorrectly field";
                    textBoxEmail.Background = Brushes.DarkRed;
                }
                else
                {
                    textBoxEmail.ToolTip = "";
                    textBoxEmail.Background = Brushes.Transparent;
                }
            }
            else
            {
                // Create a new user
                User user = new User(login, pass, email, comboBoxSavingData);
                user.Login = login; 
                user.Pass = pass;   
                user.Email = email;
                user.ComboBoxSavingData = comboBoxSavingData;

                // registration
                if (db.try_registration(user))
                {
                    MessageBox.Show("Registration was successful!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    Close();
                }
                
            }
        }

        // Registration attempt by pressing ENTER
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.Key == Key.Enter)
            {
                // Call the method for registration
                Button_Reg_Click(sender, e);
            }
        }

        // Go to the authorization window
        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
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


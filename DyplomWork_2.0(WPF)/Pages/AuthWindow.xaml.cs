using DyplomWork_2._0_WPF_.Pages;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DyplomWork_2._0_WPF_
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        DB db;
        public User authUser { get; private set; }
        public AuthWindow()
        {
            InitializeComponent();
            authUser = new User();
            db = new DB();

            // Add PreviewKeyDown event handler
            PreviewKeyDown += WindowPreviewKeyDown;

        }

        // Authorization attempt with button click
        private void ButtonAuthClick(object sender, RoutedEventArgs e)
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
                authUser.Login = login;
                authUser.Pass = pass;


                if (db.TryAutorization(authUser))
                {
                    Pages.MainWindow mainWindow = new MainWindow(authUser);
                    mainWindow.Show();
                    Close();
                }
            }
        }

        // Authorization attempt by pressing ENTER
        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.Key == Key.Enter)
            {
                // Call the method for authorization
                ButtonAuthClick(sender, e);
            }
        }

        // Go to the registration window
        private void ButtonWindowRegClick(object sender, RoutedEventArgs e)
        {
            RegWindow regWindow = new RegWindow();   
            regWindow.Show();
            Close();
        }

        // Start: Button Close | Restore | Minimize 
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnRestoreClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void BtnMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // End: Button Close | Restore | Minimize
    }
}

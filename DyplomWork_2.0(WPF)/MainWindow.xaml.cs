using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DyplomWork_2._0_WPF_.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Button selectedButton;

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Deselect all menu buttons
            foreach (var button in MenuContainer.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#437921"));
            }

            // Set the background for the current button
            clickedButton.Background = new SolidColorBrush(Colors.White);
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



        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool IsMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Deselect all menu buttons
            foreach (var button in MenuContainer.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#437921"));
            }

            // Set the background for the current button
            clickedButton.Background = new SolidColorBrush(Colors.White);

            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            Close();
        }
    }
}

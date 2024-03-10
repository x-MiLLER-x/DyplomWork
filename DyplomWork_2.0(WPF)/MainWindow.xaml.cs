using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
using static DyplomWork_2._0_WPF_.GenerationTask;
using System.IO;
using DnsClient;
using Microsoft.Graph.Models;
using MongoDB.Bson;
using System.Collections;

namespace DyplomWork_2._0_WPF_.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        DB db = new DB();
        private User authUser;

        // Define collections
        public ObservableCollection<Measurement> Ages { get; set; }
        public ObservableCollection<Measurement> Weights { get; set; }
        public ObservableCollection<Measurement> Heights { get; set; }

        public MainWindow(User user)
        {
            InitializeComponent();
            // data of authUser from authWindow
            authUser = user;

            comboBoxAgePageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxGenderPageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxWeightPageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxHeightPageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxCountryPageUser.SelectionChanged += ComboBox_SelectionChanged;

            // Initialize collections

            Ages = new ObservableCollection<Measurement>(Enumerable.Range(18, 103).Select(value => new Measurement { Value = value, Unit = "years" }));

            Weights = new ObservableCollection<Measurement>(Enumerable.Range(20, 181).Select(value => new Measurement { Value = value, Unit = "kg" }));

            Heights = new ObservableCollection<Measurement>(Enumerable.Range(100, 151).Select(value => new Measurement { Value = value, Unit = "cm" }));


            List<string> countries = new List<string>
            {"Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahrain", "Bangladesh", "Barbados", "Belarus",
                "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde",
                "Central African Republic", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo (Democratic Republic)", "Congo (Republic)", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark",
                "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany",
                "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Ivory Coast",
                "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar",
                "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar",
                "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea", "North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay",
                "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Rwanda", "Salvador", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles",
                "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Sweden", "Switzerland",
                "Syria", "Tajikistan", "Tanzania", "Thailand", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay",
                "USA", "Uzbekistan", "Vanuatu", "Vatican", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe" };

            // Binding collections
            comboBoxAge.ItemsSource = Ages;
            comboBoxWeight.ItemsSource = Weights;
            comboBoxHeight.ItemsSource = Heights;
            comboBoxCountry.ItemsSource = countries;
            comboBoxAgePageUser.ItemsSource = Ages;
            comboBoxWeightPageUser.ItemsSource = Weights;
            comboBoxHeightPageUser.ItemsSource = Heights;
            comboBoxCountryPageUser.ItemsSource = countries;

            Loaded += MainWindow_Loaded;
        }

        // Start: Loaded data
        #region LoadedData
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            authUser = db.updateUsersDataFromDB(authUser);

            // Check if comboBoxSavingData flag is set to true
            if (authUser.ComboBoxSavingData)
            {
                // Automatically filling comboboxes with authUser data
                UpdateComboBoxesWithData(authUser);
            }
        }

        private void UpdateComboBoxesWithData(User user)
        {
            SetComboBoxSelectedItem(comboBoxGender, user.Gender);
            SetComboBoxSelectedItem(comboBoxAge, user.Age, "years");
            SetComboBoxSelectedItem(comboBoxWeight, user.Weight, "kg");
            SetComboBoxSelectedItem(comboBoxHeight, user.Height, "cm");
            comboBoxCountry.SelectedItem = user.Country;

            SetComboBoxSelectedItem(comboBoxGenderPageUser, user.Gender);
            SetComboBoxSelectedItem(comboBoxAgePageUser, user.Age, "years");
            SetComboBoxSelectedItem(comboBoxWeightPageUser, user.Weight, "kg");
            SetComboBoxSelectedItem(comboBoxHeightPageUser, user.Height, "cm");
            comboBoxCountryPageUser.SelectedItem = user.Country;

            // After updating, update the Save button state
            UpdateSaveButtonState();
        }

        private void SetComboBoxSelectedItem(ComboBox comboBox, int value, string unit)
        {
            var measurement = comboBox.Items.OfType<Measurement>().FirstOrDefault(item => item.Value == value && item.Unit == unit);
            comboBox.SelectedItem = measurement;
        }

        private void SetComboBoxSelectedItem(ComboBox comboBox, string value)
        {
            var comboBoxItem = comboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content?.ToString() == value);
            comboBox.SelectedItem = comboBoxItem;
        }
        #endregion LoadedData
        // End: Loaded data
        
        // Start: Page 2
        #region page 2
        private void ShowProgressBar(bool show)
        {
            progressBar.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            blurBackground.Effect.SetValue(System.Windows.Media.Effects.BlurEffect.RadiusProperty, show ? 10.0 : 0.0);
            scrollViewer.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }
        private async Task<string> Generate_Meal_Recomendations()
        {
            Measurement selectedAge = comboBoxAge.SelectedItem as Measurement;
            int age = selectedAge?.Value ?? 0;

            string gender = comboBoxGender.SelectedItem?.ToString();
            gender = gender?.Split(':').Last().Trim();

            Measurement selectedWeight = comboBoxWeight.SelectedItem as Measurement;
            int weight = selectedWeight?.Value ?? 0;

            Measurement selectedHeight = comboBoxHeight.SelectedItem as Measurement;
            int height = selectedHeight?.Value ?? 0;
            string country = comboBoxCountry.SelectedValue.ToString();

            GenerationTask generationTask = new GenerationTask();

            string response = await OpenAIComplete(apiKey, endpointURL, modelType, maxTokens, temperature, age, gender, weight, height, country);

            TextCompletionResponse question = JsonConvert.DeserializeObject<TextCompletionResponse>(response);
            string answer = question.Choices[0].Text;

            return answer.TrimStart('\n', '\r', ' ');

        }
        private async void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
            // Check if all required data is selected
            bool isValid = true;

            if (comboBoxAge.SelectedValue == null)
            {
                comboBoxAge.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxAge.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxGender.SelectedValue == null)
            {
                comboBoxGender.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxGender.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxHeight.SelectedValue == null)
            {
                comboBoxHeight.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxHeight.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxWeight.SelectedValue == null)
            {
                comboBoxWeight.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxWeight.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxCountry.SelectedValue == null)
            {
                comboBoxCountry.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxCountry.ClearValue(Border.BorderBrushProperty);
            }

            if (!isValid)
            {
                MessageBox.Show("Please select all required data before generating meal recommendations.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShowProgressBar(true); // Show ProgressBar before performing the operation

            // Pass the generated code to the field
            textGenerated.Text = await Generate_Meal_Recomendations();

            ShowProgressBar(false); // Hide the ProgressBar after the operation is complete

            textHeader.Text = "Generated meal:";
            buttonGeneration.Content = "Regenerated";

            // Show ScrollViewer and TextBlock
            blurBackground.Height = 450;
            comboBoxGender.Visibility = Visibility.Collapsed;
            comboBoxAge.Visibility = Visibility.Collapsed;
            comboBoxWeight.Visibility = Visibility.Collapsed;
            comboBoxHeight.Visibility = Visibility.Collapsed;
            comboBoxCountry.Visibility = Visibility.Collapsed;
            buttonSave.Visibility = Visibility.Visible;
            buttonSave.IsEnabled = true;
            buttonChangeData.Visibility = Visibility.Visible;
            buttonChangeData.IsEnabled = true;
            scrollViewer.Visibility = Visibility.Visible;

        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            /////////////////////////////////////////////////////////////////////////////////////////
        }
        private void buttonChangeData_Click(object sender, RoutedEventArgs e)
        {
            // Hide ScrollViewer and TextBlock
            blurBackground.Height = 350;
            textHeader.Text = "Generate meal";
            buttonGeneration.Content = "Generate";
            comboBoxGender.Visibility = Visibility.Visible;
            comboBoxAge.Visibility = Visibility.Visible;
            comboBoxWeight.Visibility = Visibility.Visible;
            comboBoxHeight.Visibility = Visibility.Visible;
            comboBoxCountry.Visibility = Visibility.Visible;
            buttonSave.Visibility = Visibility.Hidden;
            buttonSave.IsEnabled = false;
            buttonChangeData.Visibility = Visibility.Hidden;
            buttonChangeData.IsEnabled = false;
            scrollViewer.Visibility = Visibility.Collapsed;
        }
        #endregion page 2
        // End: Page 2


        // Start: Page 3
        #region page 3
        private void buttonPageUserSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if all required data is selected
            bool isValid = true;

            if (comboBoxAgePageUser.SelectedValue == null)
            {
                comboBoxAgePageUser.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxAgePageUser.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxGenderPageUser.SelectedValue == null)
            {
                comboBoxGenderPageUser.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxGenderPageUser.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxHeightPageUser.SelectedValue == null)
            {
                comboBoxHeightPageUser.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxHeightPageUser.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxWeightPageUser.SelectedValue == null)
            {
                comboBoxWeightPageUser.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxWeightPageUser.ClearValue(Border.BorderBrushProperty);
            }

            if (comboBoxCountryPageUser.SelectedValue == null)
            {
                comboBoxCountryPageUser.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                comboBoxCountryPageUser.ClearValue(Border.BorderBrushProperty);
            }

            if (!isValid)
            {
                MessageBox.Show("Please select all required data before generating meal recommendations.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Measurement selectedAge = comboBoxAgePageUser.SelectedItem as Measurement;
            authUser.Age = selectedAge?.Value ?? 0;

            string gender = comboBoxGenderPageUser.SelectedItem?.ToString();
            gender = gender?.Split(':').Last().Trim();
            authUser.Gender = gender;

            Measurement selectedWeight = comboBoxWeightPageUser.SelectedItem as Measurement;
            authUser.Weight = selectedWeight?.Value ?? 0;

            Measurement selectedHeight = comboBoxHeightPageUser.SelectedItem as Measurement;
            authUser.Height = selectedHeight?.Value ?? 0;

            authUser.Country = comboBoxCountryPageUser.SelectedValue.ToString();

            authUser.ComboBoxSavingData = true;

            if (db.try_saving_details(authUser))
            {
                Loaded += MainWindow_Loaded;
                MessageBox.Show("Saving data was successful!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            buttonSavePageUser.IsEnabled = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSaveButtonState();
        }

        private bool IsComboBoxItemSelected(ComboBox comboBox)
        {
            return comboBox.SelectedValue != null;
        }

        private void UpdateSaveButtonState()
        {
            Measurement Age = comboBoxAgePageUser.SelectedItem as Measurement;
            int selectedAge = Age?.Value ?? 0;

            string selectedGender = comboBoxGenderPageUser.SelectedItem?.ToString();
            selectedGender = selectedGender?.Split(':').Last().Trim();

            Measurement Weight = comboBoxWeightPageUser.SelectedItem as Measurement;
            int selectedWeight = Weight?.Value ?? 0;

            Measurement Height = comboBoxHeightPageUser.SelectedItem as Measurement;
            int selectedHeight = Height?.Value ?? 0;

            bool valuesChanged = IsComboBoxItemSelected(comboBoxGenderPageUser) && authUser.Gender == selectedGender &&
                                 IsComboBoxItemSelected(comboBoxAgePageUser) && authUser.Age == selectedAge &&
                                 IsComboBoxItemSelected(comboBoxWeightPageUser) && authUser.Weight == selectedWeight &&
                                 IsComboBoxItemSelected(comboBoxHeightPageUser) && authUser.Height == selectedHeight &&
                                 IsComboBoxItemSelected(comboBoxCountryPageUser) && authUser.Country == comboBoxCountryPageUser.SelectedValue.ToString();

            buttonSavePageUser.IsEnabled = !valuesChanged;
        }

        #endregion page 3
        // End: Page 3



        // Start: Menu
        #region Menu
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
        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Deselect all menu buttons
            foreach (var button in MenuContainer.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#437921"));
            }

            // Set the background for the current button
            clickedButton.Background = new SolidColorBrush(Colors.White);

            authUser = db.updateUsersDataFromDB(authUser);

            // Check if comboBoxSavingData flag is set to true
            if (authUser.ComboBoxSavingData)
            {
                // Automatically filling comboboxes with authUser data
                UpdateComboBoxesWithData(authUser);
            }
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Deselect all menu buttons
            foreach (var button in MenuContainer.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#437921"));
            }

            // Set the background for the current button
            clickedButton.Background = new SolidColorBrush(Colors.White);

            authUser = db.updateUsersDataFromDB(authUser);

            // Check if comboBoxSavingData flag is set to true
            if (authUser.ComboBoxSavingData)
            {
                // Automatically filling comboboxes with authUser data
                UpdateComboBoxesWithData(authUser);
            }
        }
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
        #endregion Menu
        // End: Menu

        // Start: Button Close | Restore | Minimize 
        #region Button Close | Restore | Minimize 
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


        #endregion Button Close | Restore | Minimize 
        // End: Button Close | Restore | Minimize




    }
}

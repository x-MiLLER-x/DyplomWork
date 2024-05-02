using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static DyplomWork_2._0_WPF_.GenerationTask;
using System.Windows.Media.Animation;

namespace DyplomWork_2._0_WPF_.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        // Start: Loaded data
        #region LoadedData
        DB db = new DB();
        private User authUser;
        private string generatedMealText;
        private string generatedImageUrl;

        // Define collections
        public ObservableCollection<Measurement> Ages { get; set; }
        public ObservableCollection<Measurement> Weights { get; set; }
        public ObservableCollection<Measurement> Heights { get; set; }

        public ObservableCollection<ImageItem> ImageItems { get; set; }

        public MainWindow(User user)
        {
            InitializeComponent();
            // Set data to authUser from authWindow
            authUser = user;

            // Initializing an Image Collection
            ImageItems = new ObservableCollection<ImageItem>();


            // Binding the ImageItems collection to ItemsControl in XAML
            DataContext = this;

            comboBoxAgePageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxGenderPageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxWeightPageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxHeightPageUser.SelectionChanged += ComboBox_SelectionChanged;
            comboBoxCountryPageUser.SelectionChanged += ComboBox_SelectionChanged;

            // Initialize collections. Ages, Weight, Height, Countries
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

            // Update data
            Loaded += MainWindow_Loaded_Update;
            LoadImages();
        }

        private async void LoadImages()
        {
            ImageItems.Clear();
            for (int i = 0; i < authUser.Images.Count; i++)
            {
                string imageUrl = authUser.Images[i];
                string meal = authUser.Meals.ElementAtOrDefault(i); // Получаем блюдо из коллекции Meals с тем же индексом

                // Create a new BitmapImage object
                BitmapImage bitmapImage = await Convert_url_to_bitmapImage(imageUrl);
                if (bitmapImage != null)
                {
                    ImageItems.Add(new ImageItem { Image = bitmapImage, Meal = meal }); // Устанавливаем URL и блюдо для ImageItem
                }
                else
                {
                    Console.WriteLine($"Failed to load image: {imageUrl}"); // Output to console for debugging
                }
            }
        }
        private void SetBackgroundImage(ImageBrush targetBrush, string imageUrl)
        {
            // Create a new BitmapImage object
            BitmapImage bitmapImage = new BitmapImage();
            // Set the image source by URL
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUrl);
            bitmapImage.EndInit();
            // Set the image as the source for the ImageBrush
            targetBrush.ImageSource = bitmapImage;
        }

        // Window loading animation
        private void Window_Loading_Animation(object sender, RoutedEventArgs e)
        {
            // Creating Animation
            DoubleAnimation translateYAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            // Setting Animation Target
            Storyboard.SetTargetName(translateYAnimation, "page1");
            Storyboard.SetTargetProperty(translateYAnimation, new PropertyPath("(Border.RenderTransform).(TranslateTransform.Y)"));

            // Creating and running animation
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(translateYAnimation);
            storyboard.Begin(this);
        }


        // Update data of user from DB. Update data in comboBoxes
        private void MainWindow_Loaded_Update(object sender, RoutedEventArgs e)
        {
            authUser = db.updateUsersDataFromDB(authUser);

            // Check if comboBoxSavingData flag is set to true
            if (authUser.ComboBoxSavingData)
            {
                // Automatically filling comboboxes with authUser data
                UpdateComboBoxesWithData(authUser);
            }
        }

        // Change every comboBoxes data by data from user
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

        // Replacement for int data from database
        private void SetComboBoxSelectedItem(ComboBox comboBox, int value, string unit)
        {
            var measurement = comboBox.Items.OfType<Measurement>().FirstOrDefault(item => item.Value == value && item.Unit == unit);
            comboBox.SelectedItem = measurement;
        }

        // Replacement for string data from database
        private void SetComboBoxSelectedItem(ComboBox comboBox, string value)
        {
            var comboBoxItem = comboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content?.ToString() == value);
            comboBox.SelectedItem = comboBoxItem;
        }
        #endregion LoadedData
        // End: Loaded data

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
        private void savedMealButton_Click(object sender, RoutedEventArgs e)
        {
            LoadImages();
            Button clickedButton = (Button)sender;

            // Deselect all menu buttons
            foreach (var button in MenuContainer.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#437921"));
            }

            // Set the background for the current button
            clickedButton.Background = new SolidColorBrush(Colors.White);

            // Update data from DB
            authUser = db.updateUsersDataFromDB(authUser);

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

        // Start: Page 1
        #region page 1
        private void LearnMore_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // URL, который нужно открыть
                string url = "https://www.nutritionix.com/database/common-foods";

                try
                {
                    // Открыть URL в браузере по умолчанию
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, если не удалось открыть URL
                    MessageBox.Show($"Error opening URL: {ex.Message}");
                }
            }
        }
        #endregion page 1
        // End: Page 1

        // Start: Page 2
        #region page 2
        // Progress bar appearance
        private void ShowProgressBar(bool show)
        {
            progressBar.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            blurBackground.Effect.SetValue(System.Windows.Media.Effects.BlurEffect.RadiusProperty, show ? 10.0 : 0.0);
            scrollViewer.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        // Generate meal
        private async Task<string> Generate_Meal_Recomendations()
        {
            // Take the data from comboboxes
            Measurement selectedAge = comboBoxAge.SelectedItem as Measurement;
            int age = selectedAge?.Value ?? 0;

            string gender = comboBoxGender.SelectedItem?.ToString();
            gender = gender?.Split(':').Last().Trim();

            Measurement selectedWeight = comboBoxWeight.SelectedItem as Measurement;
            int weight = selectedWeight?.Value ?? 0;

            Measurement selectedHeight = comboBoxHeight.SelectedItem as Measurement;
            int height = selectedHeight?.Value ?? 0;
            string country = comboBoxCountry.SelectedValue.ToString();


            // Generate answer
            GenerationTask generationTask = new GenerationTask();

            string response = await OpenAIComplete(apiKey, endpointURL, modelText, maxTokens, temperature, age, gender, weight, height, country);

            TextCompletionResponse question = JsonConvert.DeserializeObject<TextCompletionResponse>(response);
            string answer = question.Choices[0].Text;

            // Return answer
            return answer.TrimStart('\n', '\r', ' ', '.', ',');

        }

        // Button_Generate_Click action
        private async void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
            // Check if all required data is selected
            bool isValid = true;

            // Checking if all required data is selected
            if (comboBoxAge.SelectedValue == null || comboBoxGender.SelectedValue == null ||
                comboBoxHeight.SelectedValue == null || comboBoxWeight.SelectedValue == null ||
                comboBoxCountry.SelectedValue == null)
            {
                // Set a red frame for unselected elements
                comboBoxAge.BorderBrush = comboBoxAge.SelectedValue == null ? Brushes.Red : null;
                comboBoxGender.BorderBrush = comboBoxGender.SelectedValue == null ? Brushes.Red : null;
                comboBoxHeight.BorderBrush = comboBoxHeight.SelectedValue == null ? Brushes.Red : null;
                comboBoxWeight.BorderBrush = comboBoxWeight.SelectedValue == null ? Brushes.Red : null;
                comboBoxCountry.BorderBrush = comboBoxCountry.SelectedValue == null ? Brushes.Red : null;

                isValid = false;
            }
            else
            {
                // Clearing frames for selected elements
                comboBoxAge.ClearValue(Border.BorderBrushProperty);
                comboBoxGender.ClearValue(Border.BorderBrushProperty);
                comboBoxHeight.ClearValue(Border.BorderBrushProperty);
                comboBoxWeight.ClearValue(Border.BorderBrushProperty);
                comboBoxCountry.ClearValue(Border.BorderBrushProperty);
            }

            if (!isValid)
            {
                MessageBox.Show("Please select all required data before generating meal recommendations.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShowProgressBar(true); // Show ProgressBar before performing the operation

            try
            {
                // Get the generated text
                generatedMealText = await Generate_Meal_Recomendations();

                // Put the generated text in textGenerated field
                textGenerated.Text = generatedMealText;

                // Generate an image by the generated text as a prompt
                var generatedImageResponse = await GenerateImageFromPrompt(generatedMealText);

                generatedImageUrl = generatedImageResponse.ImageUrl;

                // Set the image as background
                SetBackgroundImage(imagePage2, generatedImageUrl);
                borderPage2.Background = new SolidColorBrush(Colors.Transparent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate or load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Hiding the ProgressBar after the operation is completed
                ShowProgressBar(false);
            }

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

        // Generate image func
        private async Task<ImageGenerationResponse> GenerateImageFromPrompt(string prompt)
        {
            // call the image generation function, passing the generated text as prompt
            var imageResponse = await GenerationTask.OpenAIGenerateImage(GenerationTask.apiKey, prompt, GenerationTask.modelImage, GenerationTask.numImages, GenerationTask.imageSize);

            return imageResponse;
        }

        // Substitution generated image
        private Task<BitmapImage> Convert_url_to_bitmapImage(string imageUrl)
        {
            // Create a new BitmapImage object
            BitmapImage bitmapImage = new BitmapImage();
            // Set the image source by URL
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUrl);
            bitmapImage.EndInit();
            return Task.FromResult(bitmapImage);
        }

        // buttonChangeData_Click action
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

        // buttonSave_Click action
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            authUser.AnySavedMeal = true;
            // Check if generatedMealText and generatedImageUrl are not null or empty
            if (!string.IsNullOrEmpty(generatedMealText) && !string.IsNullOrEmpty(generatedImageUrl))
            {
                // Add generated meal text and image URL to user's lists
                authUser.Meals.Add(generatedMealText);
                authUser.Images.Add(generatedImageUrl);

                // Save user details
                if (db.try_saving_details(authUser))
                {
                    MessageBox.Show("Saving data was successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                else
                {
                    MessageBox.Show("Failed to save data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No generated data to save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion page 2
        // End: Page 2

        // Start: Page 3
        #region page 3
        private void buttonPageUserSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if all required data is selected
            bool isValid = true;

            // Checking if all required data is selected
            if (comboBoxAgePageUser.SelectedValue == null || comboBoxGenderPageUser.SelectedValue == null ||
                comboBoxHeightPageUser.SelectedValue == null || comboBoxWeightPageUser.SelectedValue == null ||
                comboBoxCountryPageUser.SelectedValue == null)
            {
                // Set a red frame for unselected elements
                comboBoxAgePageUser.BorderBrush = comboBoxAgePageUser.SelectedValue == null ? Brushes.Red : null;
                comboBoxGenderPageUser.BorderBrush = comboBoxGenderPageUser.SelectedValue == null ? Brushes.Red : null;
                comboBoxHeightPageUser.BorderBrush = comboBoxHeightPageUser.SelectedValue == null ? Brushes.Red : null;
                comboBoxWeightPageUser.BorderBrush = comboBoxWeightPageUser.SelectedValue == null ? Brushes.Red : null;
                comboBoxCountryPageUser.BorderBrush = comboBoxCountryPageUser.SelectedValue == null ? Brushes.Red : null;

                isValid = false;
            }
            else
            {
                // Clearing frames for selected elements
                comboBoxAgePageUser.ClearValue(Border.BorderBrushProperty);
                comboBoxGenderPageUser.ClearValue(Border.BorderBrushProperty);
                comboBoxHeightPageUser.ClearValue(Border.BorderBrushProperty);
                comboBoxWeightPageUser.ClearValue(Border.BorderBrushProperty);
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
                Loaded += MainWindow_Loaded_Update;
                MessageBox.Show("Saving data was successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        // Start: Page 4
        #region page 4
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Получаем элемент System.Windows.Controls.Image, на который было нажатие
            System.Windows.Controls.Image clickedImage = (System.Windows.Controls.Image)sender;

            // Получаем родительский элемент StackPanel
            StackPanel parentStackPanel = (StackPanel)clickedImage.Parent;

            // Получаем DataContext StackPanel, чтобы получить ImageItem
            ImageItem clickedImageItem = (ImageItem)parentStackPanel.DataContext;

            // Обновляем содержимое TextBox
            savedMeals.Text = clickedImageItem.Meal;

            // Прячем imagePanel и показываем scrollViewerSavedMeals
            imagePanel.Visibility = Visibility.Hidden;
            scrollViewerSavedMeals.Visibility = Visibility.Visible;
            scrollViewerSavedImages.Visibility = Visibility.Hidden;
            buttonBack.Visibility = Visibility.Visible;
        }
    
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            // Прячем imagePanel и показываем scrollViewerSavedMeals
            imagePanel.Visibility = Visibility.Visible;
            scrollViewerSavedMeals.Visibility = Visibility.Hidden;
            scrollViewerSavedImages.Visibility = Visibility.Visible;
            buttonBack.Visibility = Visibility.Hidden;
        }
        #endregion page 4
        // End: Page 4

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

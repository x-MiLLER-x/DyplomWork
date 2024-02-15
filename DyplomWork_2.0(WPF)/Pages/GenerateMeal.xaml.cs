using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using static DyplomWork_2._0_WPF_.GenerationTask;

namespace DyplomWork_2._0_WPF_.Pages
{
    /// <summary>
    /// Interaction logic for GenerateMeal.xaml
    /// </summary>
    public partial class GenerateMeal : Page
    {
        // Define collections
        public ObservableCollection<int> Ages { get; set; }
        public ObservableCollection<int> Weights { get; set; }
        public ObservableCollection<int> Heights { get; set; }

        public GenerateMeal()
        {
            InitializeComponent();

            // Initialize collections
            Ages = new ObservableCollection<int>(Enumerable.Range(18, 103)); // from 18 to 120 years
            Weights = new ObservableCollection<int>(Enumerable.Range(20, 181)); // from 20 to 200 kg
            Heights = new ObservableCollection<int>(Enumerable.Range(100, 151)); // from 100 to 250 cm
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
        }

        private async Task<string> Generate_Meal_Recomendations()
        {
            double age = Convert.ToDouble(comboBoxAge.SelectedValue);
            string gender = comboBoxGender.SelectedValue.ToString();
            double weight = Convert.ToDouble(comboBoxWeight.SelectedValue);
            double height = Convert.ToDouble(comboBoxHeight.SelectedValue);
            string country = comboBoxCountry.SelectedValue.ToString();


            GenerationTask generationTask = new GenerationTask();

            string response = await OpenAIComplete(apiKey, endpointURL, modelType, maxTokens, temperature, age, gender, weight, height, country);

            TextCompletionResponse question = JsonConvert.DeserializeObject<TextCompletionResponse>(response);
            string answer = question.Choices[0].Text;

            return answer.TrimStart('\n', '\r');

        }

        private void ShowProgressBar(bool show)
        {
            progressBar.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            blurBackground.Effect.SetValue(System.Windows.Media.Effects.BlurEffect.RadiusProperty, show ? 10.0 : 0.0);
            scrollViewer.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
            ShowProgressBar(true); // Show ProgressBar before performing the operation

            // Pass the generated code to the field
            textGenerated.Text = await Generate_Meal_Recomendations();

            ShowProgressBar(false); // Hide the ProgressBar after the operation is complete

            textHeader.Text = "Generated meal:";
            buttonGeneration.Content = "Regenerated";

            // Show ScrollViewer and TextBlock
            comboBoxGender.Visibility = Visibility.Collapsed;
            comboBoxAge.Visibility = Visibility.Collapsed;
            comboBoxWeight.Visibility = Visibility.Collapsed;
            comboBoxHeight.Visibility = Visibility.Collapsed;
            comboBoxCountry.Visibility = Visibility.Collapsed;
            buttonSave.Visibility = Visibility.Visible;
            buttonSave.IsEnabled = true;
            scrollViewer.Visibility = Visibility.Visible;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            // Add your save logic here
        }
    }
}

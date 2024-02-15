using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DyplomWork_2._0_WPF_.GenerationTask;

namespace DyplomWork_2._0_WPF_.Pages
{
    /// <summary>
    /// Interaction logic for GenerateMeal.xaml
    /// </summary>
    public partial class GenerateMeal : Page
    {
        public GenerateMeal()
        {
            InitializeComponent();
        }
        private async Task<string> Generate_Meal_Recomendations()
        {
            double age = 20;
            string gender = "Male";
            double weight = 80;
            double height = 180;

            GenerationTask generationTask = new GenerationTask();

            string response = await OpenAIComplete(apiKey, endpointURL, modelType, maxTokens, temperature, age, gender, weight, height);

            TextCompletionResponse question = JsonConvert.DeserializeObject<TextCompletionResponse>(response);
            string answer = question.Choices[0].Text;

            return answer.TrimStart('\n');

        }
        private async void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
            textGenerated.Text = await Generate_Meal_Recomendations();

            // Получаем текущую высоту экрана
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            textHeader.Text = "Generated meal:";
            buttonGeneration.Content = "Regenerated";

            // Показываем ScrollViewer и TextBlock
            buttonSave.Visibility = Visibility.Visible;
            buttonSave.IsEnabled = true;
            scrollViewer.Visibility = Visibility.Visible;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

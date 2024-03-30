using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DyplomWork_2._0_WPF_
{
    internal class GenerationTask
    {
        private static readonly HttpClient client = new HttpClient();

        // Supporting Classes
        public class Choice
        {
            public string Text { get; set; }
        }

        public class TextCompletionResponse
        {
            public Choice[] Choices { get; set; }
        }

        public class ImageGenerationResponse
        {
            public string ImageUrl { get; set; }
        }

        // Supporting Data
        public static string apiKey = "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";
        public static string endpointURL = "https://api.openai.com/v1/completions";
        public static string imageGenerationURL = "https://api.openai.com/v1/images/generations";
        public static string modelType = "gpt-3.5-turbo-instruct";
        public static int maxTokens = 1000;
        public static double temperature = 1.0f;

        // Generating meal
        public static async Task<string> OpenAIComplete(string apikey, string endpoint, string modeltype, int maxtokens, double temp, int age, string gender, int weight, int height, string country)
        {
            var requestbody = new
            {
                model = modeltype,
                prompt = "I am from" + country + ". I am " + age + ". My gender is " + gender + ". My weight is " + weight + " kg, my height is " + height + " cm. " +
                "Write a very healthy meal for a one week." +
                "Use the Department of Health's recommendations for calculating calories for the day." +
                "All calories in each dish must be counted." +
                "There is should be tipical dishes for my country." +
                "Write like this:" + "\n" +
                "Monday:" + "\n" +
                "1. Breakfast - ..., " + "\n" +
                "2. Lunch - ..., " + "\n" +
                "3. Dinner - ..., " + "\n" +
                "4. Supper - ...",
                max_tokens = maxtokens,
                temperature = temp
            };

            // Serialize the payload to JSON
            string jsonPayload = JsonConvert.SerializeObject(requestbody);

            // Create the HTTP request
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Add("Authorization", $"Bearer {apikey}");
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the request and get the response
            var httpResponse = await client.SendAsync(request);
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            return responseContent;
        }
        // Generating image
        public static async Task<ImageGenerationResponse> OpenAIGenerateImage(string apikey, string prompt, string modelType, int numImages, string imageSize)
        {
            var requestbody = new
            {
                model = modelType,
                prompt = prompt,
                n = numImages,
                size = imageSize
            };

            // Serialize the payload to JSON
            string jsonPayload = JsonConvert.SerializeObject(requestbody);

            // Create the HTTP request
            var request = new HttpRequestMessage(HttpMethod.Post, imageGenerationURL);
            request.Headers.Add("Authorization", $"Bearer {apikey}");
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the request and get the response
            var httpResponse = await client.SendAsync(request);
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Deserialize the response to get the generated image URL
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string imageUrl = jsonResponse.data[0].url;

            //Console.WriteLine("Generated image URL:");
            //Console.WriteLine(imageUrl);
            //Console.WriteLine("Response from API OpenAI:");
            //Console.WriteLine(responseContent);

            // Return the ImageGenerationResponse object with the image URL
            return new ImageGenerationResponse { ImageUrl = imageUrl };
        }
    }
}

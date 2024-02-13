﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace DyplomWork_2._0_WPF_
{
    internal class GenerationTask
    {
        private static readonly HttpClient client = new HttpClient();

        public class Choice
        {
            public string Text { get; set; }
        }

        public class TextCompletionResponse
        {
            public Choice[] Choices { get; set; }
        }

        #region public static string apiKey = "";

        public static string apiKey = "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";

        #endregion

        public static string endpointURL = "https://api.openai.com/v1/completions";
        public static string modelType = "gpt-3.5-turbo-instruct";
        public static int maxTokens = 1000;
        public static double temperature = 1.0f;



        public static async Task Main(string[] args)
        {
            double age = 20;
            string gender = "Male";
            double weight = 80;
            double height = 180;
            string response = await OpenAIComplete(apiKey, endpointURL, modelType, maxTokens, temperature, age, gender, weight, height);

            TextCompletionResponse question = JsonConvert.DeserializeObject<TextCompletionResponse>(response);
            string answer = question.Choices[0].Text;

            Console.WriteLine(answer);
        }


        public static async Task<string> OpenAIComplete(string apikey, string endpoint, string modeltype, int maxtokens, double temp, double age, string gender, double weight, double height)
        {
            var requestbody = new
            {
                model = modeltype,
                prompt = "I am from Ukraine. I am " + age + ". My gender is " + gender + ". My weight is " + weight + " kg, my height is " + height + " cm. " +
                "Write a very healthy meal for a one week." +
                "Use the Department of Health's recommendations for calculating calories for the day." +
                "There is should be tipical dishes for my country." +
                "Write like this:" + "/n" +
                "Monday:" + "/n" +
                "1. Breakfast - ..., " + "/n" +
                "2. Lunch - ..., " + "/n" +
                "3. Dinner - ..., " + "/n" +
                "4. Supper - ...",
                max_tokens = maxtokens,
                temperature = temp
            };

            // Serialize the payload to JSON
            string jsonPayload = JsonConvert.SerializeObject(requestbody);

            // Create the HTTP request
            var request = new HttpRequestMessage(HttpMethod.Post, endpointURL);
            request.Headers.Add("Authorization", $"Bearer {apikey}");
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the request and get the response
            var httpResponse = await client.SendAsync(request);
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}
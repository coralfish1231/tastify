using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace ConsoleCSEx6
{
    public class ApiResponse
    { // for page results for when we make pages
        [JsonProperty("results")]
        public List<Business> Results { get; set; }

        [JsonProperty("resultCount")]
        public int ResultCount { get; set; }
    }
    // all business info
    public class Business
    {
        [JsonProperty("bizId")]
        public string BizId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("Images")]
        public List<string> Images { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }
    }
    

    namespace ConsoleCSEx6
    {
        public class Program
        {
            public static async Task Main(string[] args)
            {
                Console.Write("Enter a City >>> ");
                string city = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(city))
                {
                    Console.WriteLine("City cannot be empty!");
                    return;
                }

                Console.Write("Enter a Food Category (ex, pizza, sushi, tacos) >>> ");
                string category = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(category))
                {
                    Console.WriteLine("Category cannot be empty!");
                    return;
                }
                 
                string url = $"https://yelp-business-reviews.p.rapidapi.com/search?location={Uri.EscapeDataString(city)}&query={Uri.EscapeDataString(category)}";

                using (HttpClient client = new HttpClient())
                {   //this is our URL and API key 
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "3f063b706fmsh757cc59f38264b4p14be39jsned4f81524c86");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "yelp-business-reviews.p.rapidapi.com");

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        string responseBody = await response.Content.ReadAsStringAsync();

                        Console.WriteLine("Raw Response:");
                        Console.WriteLine(responseBody);

                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error: {response.StatusCode}");
                            return;
                        }

                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                        if (apiResponse?.Results == null || apiResponse.Results.Count == 0)
                        {
                            Console.WriteLine("No results found.");
                            return;
                        }

                        Console.WriteLine($"\nFound {apiResponse.ResultCount} businesses for '{category}' in '{city}':\n");

                        foreach (var biz in apiResponse.Results)
                        {
                            Console.WriteLine($"Name: {biz.Name}");
                            Console.WriteLine($"Alias: {biz.Alias}");
                            Console.WriteLine($"Rating: {biz.Rating} ({biz.ReviewCount} reviews)");
                            Console.WriteLine($"Phone: {biz.Phone}");
                            Console.WriteLine($"Website: {biz.Website}");
                            Console.WriteLine("Categories: " + (biz.Categories != null ? string.Join(", ", biz.Categories) : "N/A"));
                            Console.WriteLine($"Image URL: {biz.Images}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred:\n{ex.Message}");
                    }
                }
            }
        }
    }
}
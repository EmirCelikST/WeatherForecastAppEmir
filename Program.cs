using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WmForecastEmir;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Choose your language / Dilinizi seçin:");
        Console.WriteLine("1. English");
        Console.WriteLine("2. Türkçe");

        int languageChoice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out languageChoice) && (languageChoice == 1 || languageChoice == 2))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1 for English or 2 for Türkçe.");
            }
        }

        if (languageChoice == 1)
        {
            Console.WriteLine("Which city's weather forecast would you like to see? (Istanbul, Izmir, Ankara)");
        }
        else
        {
            Console.WriteLine("Hangi şehrin hava durumu tahminlerini görmek istersiniz? (İstanbul, İzmir, Ankara)");
        }

        string cityName = Console.ReadLine().Trim().ToLower();

        if (cityName == "istanbul" || cityName == "izmir" || cityName == "ankara")
        {
            string apiUrl = $"https://goweather.herokuapp.com/weather/{cityName}";
            await GetAndDisplayWeather(apiUrl, cityName, languageChoice);
        }
        else
        {
            Console.WriteLine("Invalid city name. Please enter Istanbul, Izmir, or Ankara.");
        }
    }

    static async Task GetAndDisplayWeather(string apiUrl, string cityName, int languageChoice)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string json = await client.GetStringAsync(apiUrl);
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(json);

                cityName = char.ToUpper(cityName[0]) + cityName.Substring(1);

                Console.WriteLine($"{cityName} Weather Forecast / {cityName} Hava durumu tahminleri:");

                Console.WriteLine($"Current / Mevcut: {weatherData.Temperature}°C, Wind / Rüzgar: {weatherData.Wind}, {weatherData.Description}");

                Console.WriteLine("Forecast for the next 3 days / Sonraki 3 günün hava durumu tahminleri:");
                for (int i = 0; i < 3; i++)
                {
                    var dayForecast = weatherData.Forecast[i];
                    Console.WriteLine($"{DateTime.Now.AddDays(i).ToString("dddd, dd/MM/yyyy")}: {dayForecast.Temperature}°C, Wind / Rüzgar: {dayForecast.Wind}, {dayForecast.Description}");
                }

                Console.WriteLine();

                if (languageChoice == 1)
                {
                    Console.WriteLine("The weather forecast for the next 3 days has been provided. Have a great day!");
                }
                else
                {
                    Console.WriteLine("3 Günlük hava durumu tahminleri sunulmuştur. Keyifli bir gün geçirmeniz dileğiyle!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving weather data for {cityName} / {cityName} için hava durumu bilgilerini alırken hata oluştu: {ex.Message}");
            }
        }
    }
}
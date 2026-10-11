using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ExchangeRateService
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sale { get; set; }
        public string Buy { get; set; }
        public string CoinIcon { get; set; }
    }

    public class CityExchangeRates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; }
    }

    public class ApiResponse
    {
        public Dictionary<string, List<CityExchangeRates>> Data { get; set; }
    }
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetExchangeRatejson(){
        var response = await _httpClient.GetAsync("https://kuraimibank.com/api/ar/home");
        response.EnsureSuccessStatusCode(); // تأكد من نجاح الطلبt

        var jsonString = await response.Content.ReadAsStringAsync();
        return  jsonString;
    }

    public async Task<List<CityExchangeRates>> GetExchangeRatesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://kuraimibank.com/api/ar/home");
            response.EnsureSuccessStatusCode(); // تأكد من نجاح الطلب

            var jsonString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString);

            // تحقق من أن apiResponse و apiResponse.Data ليسوا null
            if (jsonString == null )
            {
                throw new Exception("الاستجابة من الـ API فارغة أو غير صحيحة.");
            }

            // تحقق من أن المفتاح "exchange_rates" موجود في apiResponse.Data
            if (!apiResponse.Data.ContainsKey("exchange_rates"))
            {
                throw new Exception("المفتاح 'exchange_rates' غير موجود في الاستجابة.");
            }

            return apiResponse.Data["exchange_rates"];
        }
        catch (HttpRequestException ex)
        {
            // معالجة أخطاء الاتصال بالـ API
            throw new Exception("حدث خطأ أثناء الاتصال بالـ API.", ex);
        }
        catch (JsonException ex)
        {
            // معالجة أخطاء تحليل JSON
            throw new Exception("حدث خطأ أثناء تحليل استجابة الـ API.", ex);
        }
        catch (Exception ex)
        {
            // معالجة أي أخطاء أخرى
            throw new Exception("حدث خطأ غير متوقع.", ex);
        }
    }
    public async Task<List<CityExchangeRates>> GetCityExchangeRatesAsync()
    {
        // إرسال الطلب إلى API
        var response = await _httpClient.GetAsync("https://kuraimibank.com/api/ar/home");
        response.EnsureSuccessStatusCode(); // تأكد من نجاح الطلب

        // قراءة الاستجابة كـ JSON نص
        var jsonString = await response.Content.ReadAsStringAsync();

        // تحليل النص إلى كائن JSON
        var jsonObject = JObject.Parse(jsonString);

        // استخراج بيانات exchange_rates
        var exchangeRatesArray = jsonObject["data"]["exchange_rates"];

        // تحويل البيانات إلى قائمة CityExchangeRates
        var cityExchangeRates = exchangeRatesArray
            .Select(city => new CityExchangeRates
            {
                Id = (int)city["id"],
                Name = (string)city["name"],
                ExchangeRates = city["exchange_rate"]
                    .Select(rate => new ExchangeRate
                    {
                        Id = (int)rate["id"],
                        Name = (string)rate["name"],
                        Sale = (string)rate["sale"],
                        Buy = (string)rate["buy"],
                        CoinIcon = (string)rate["coin_icon"],
                    }).ToList(),
            }).ToList();

        return cityExchangeRates;
    }
}
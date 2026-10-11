using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hoor.Web.Areas.Identity.Pages.Account
{
    public class CurrencyModel : PageModel
    {
        private readonly ExchangeRateService _exchangeRateService;

        public CurrencyModel(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        // ﬁ«∆„… «·⁄„·«  «·„—«œ ⁄—÷Â« ›Ì «·’›Õ…
        public List<Currency> Currencies { get; set; } = new List<Currency>();
        public string jsonrespons {  get; set; }
        public List<ExchangeRateService.CityExchangeRates> CityExchangeRates { get; set; }

        public class Currency
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string BuyPrice { get; set; }
            public string SellPrice { get; set; }
        }

        public async Task OnGet()
        {

            jsonrespons = await _exchangeRateService.GetExchangeRatejson();
            

            CityExchangeRates = await _exchangeRateService.GetCityExchangeRatesAsync();

            foreach (var item in CityExchangeRates)
            {
                
                    foreach (var item1 in item.ExchangeRates)
                    {
                        Currency currency = new Currency()
                        {
                            Id = item1.Id,
                            Name = item1.Name,
                            BuyPrice = item1.Buy,
                            SellPrice = item1.Sale
                        };
                        Currencies.Add(currency);
                    }
                    break;
                
               
            }
        }
    }
}

using System.Text.Json;
using TechMoveGLMS.Interfaces;

namespace TechMoveGLMS.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertCurrency(decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return 0;
                }

                var url = "https://api.frankfurter.app/latest?from=USD&to=ZAR";

                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                using JsonDocument document = JsonDocument.Parse(json);

                var rate = document.RootElement
                    .GetProperty("rates")
                    .GetProperty("ZAR")
                    .GetDecimal();

                return amount * rate;
            }
            catch
            {
                return 0;
            }
        }
    }
}
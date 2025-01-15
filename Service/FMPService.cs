using System;
using System.Net.Http;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FMPService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Stock?> FindStockBySymbolAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be null or empty.", nameof(symbol));

            try
            {
                // Build the request URL
                var requestUrl =
                    $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}";

                // Execute HTTP GET request
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"Failed to fetch stock data. HTTP Status: {response.StatusCode}"
                    );
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the response into the expected object
                var fmpStocks = JsonConvert.DeserializeObject<FMPStock[]>(content);

                // Validate the response and convert the first stock to the desired model
                var stockData = fmpStocks?.FirstOrDefault();
                return stockData?.ToStockFromFMP();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return null;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP request error: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}

using BlazorApp.Components;
using BlazorApp.Http;
using Common.Model.Models;

namespace BlazorApp.Pages
{
    public partial class HelloDatabase
    {
        private string? databaseMessage = "Click button to get response!";
        private List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();
        private string? aggregatorMessage = "";

        private Modal ConfirmDialog;

        private string weatherInput;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                ConfirmDialog.OnConfirm += async () => await AfterFetchAggregatorConfirm();
            }
        }

        private async Task FetchApiData()
        {
            // Call the API 
            using var httpClient = new HttpClient();
            var result = await httpClient.GetFromGatewayAsync<HelloDatabaseModel>("api/Hello/DatabaseTest");

            databaseMessage = result?.TimeString;
        }

        private async Task FetchWeatherData()
        {
            // Call the API 
            using var httpClient = new HttpClient();
            var result = await httpClient.GetFromGatewayAsync<WeatherForecast[]>("api/WeatherForecast/Weather");

            if(result != null)
                weatherForecasts = result.ToList();
        }

        private void FetchAggregator()
        {
            //set event handler here in the method signature or set it in OnAfterRender
            //ConfirmDialog.Open(async () => await FetchData());
            ConfirmDialog.Open();
        }

        public async Task AfterFetchAggregatorConfirm()
        {
            using var httpClient = new HttpClient();
            var result = await httpClient.GetFromGatewayAsync<WeatherDatabaseAggregateModel>("api/DatabaseWeatherAggregator");

            databaseMessage = result?.Database?.TimeString;
            weatherForecasts = result?.Weather?.ToList() ?? new List<WeatherForecast>();
            aggregatorMessage = result?.AggregateMessage;

            StateHasChanged();
        }

        public async Task AddWeatherType()
        {
            using var httpClient = new HttpClient();
            List<WeatherSummaryModel>? result = await httpClient.PostToGatewayAsync<WeatherSummaryModel, List<WeatherSummaryModel>>("api/WeatherForecast/Weather", new WeatherSummaryModel { Summary = weatherInput });

            weatherInput = "";
        }
    }
}

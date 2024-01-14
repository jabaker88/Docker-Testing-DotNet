using BlazorApp.Components;
using BlazorApp.Http;
using Common.Model.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
    public partial class HelloDatabase
    {
        [Inject]
        private IHttpClientFactory clientFactory { get; set; }
        private HttpClient? HttpClient => clientFactory.CreateClient("Gateway");

        private string? databaseMessage = "Click button to get response!";
        private List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();
        private string? aggregatorMessage = "";
        private string weatherInput = "";

        private Modal? ConfirmDialog;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                ConfirmDialog!.OnConfirm += async () => await AfterFetchAggregatorConfirm();
            }
        }

        private async Task FetchApiData()
        {
            // Call the API 
            var result = await HttpClient.GetFromGatewayAsync<HelloDatabaseModel>("api/Hello/DatabaseTest");

            databaseMessage = result?.TimeString;
        }

        private async Task FetchWeatherData()
        {
            // Call the API 
            var result = await HttpClient.GetFromGatewayAsync<WeatherForecast[]>("api/WeatherForecast/Weather");

            if(result != null)
                weatherForecasts = result.ToList();
        }

        private void FetchAggregator()
        {
            //set event handler here in the method signature or set it in OnAfterRender
            //ConfirmDialog.Open(async () => await FetchData());
            ConfirmDialog?.Open();
        }

        public async Task AfterFetchAggregatorConfirm()
        {
            var result = await HttpClient.GetFromGatewayAsync<WeatherDatabaseAggregateModel>("api/DatabaseWeatherAggregator");

            databaseMessage = result?.Database?.TimeString;
            weatherForecasts = result?.Weather?.ToList() ?? new List<WeatherForecast>();
            aggregatorMessage = result?.AggregateMessage;

            StateHasChanged();
        }

        public async Task AddWeatherType()
        {
            List<WeatherSummaryModel>? result = await HttpClient.PostToGatewayAsync<WeatherSummaryModel, List<WeatherSummaryModel>>("api/WeatherForecast/Weather", new WeatherSummaryModel { Summary = weatherInput });

            weatherInput = "";
        }
    }
}

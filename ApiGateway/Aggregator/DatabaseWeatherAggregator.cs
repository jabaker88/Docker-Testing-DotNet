using Common.Model.Models;
using MMLib.SwaggerForOcelot.Aggregates;
using Ocelot.Configuration;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Aggregator
{
    [AggregateResponse("Aggregate the Hello Database and Weather reponses", typeof(WeatherDatabaseAggregateModel))]
    public class DatabaseWeatherAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var aggregateModel = new WeatherDatabaseAggregateModel();

            foreach (var response in responses)
            {
                string downStreamRouteKey = ((DownstreamRoute)response.Items["DownstreamRoute"]).Key;
                DownstreamResponse downstreamResponse = (DownstreamResponse)response.Items["DownstreamResponse"];
                var responseContentString = Encoding.UTF8.GetString(await downstreamResponse.Content.ReadAsByteArrayAsync());

                if (downStreamRouteKey == "Hello")
                {
                    aggregateModel.Database = JsonSerializer.Deserialize<HelloDatabaseModel>(responseContentString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else if (downStreamRouteKey == "Weather")
                {
                    aggregateModel.Weather = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContentString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }

            aggregateModel.AggregateMessage = "Hello from the Database and Weather Aggregator!";

            var stringContent = new StringContent(JsonSerializer.Serialize(aggregateModel), Encoding.UTF8, "application/json");

            return new DownstreamResponse(stringContent, System.Net.HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
            //throw new NotImplementedException();
        }
    }
}

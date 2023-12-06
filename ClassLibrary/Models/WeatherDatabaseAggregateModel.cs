using System.Collections.Generic;

namespace Common.Model.Models
{
    public class WeatherDatabaseAggregateModel
    {
        public HelloDatabaseModel Database { get; set; } = new HelloDatabaseModel();
        public List<WeatherForecast> Weather { get; set; } = new List<WeatherForecast>();
        public string AggregateMessage { get; set; }
    }
}

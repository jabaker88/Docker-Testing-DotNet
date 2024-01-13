using System;
using System.Collections.Generic;

namespace Common.Model.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class WeatherSummaryModel
    {
        public string Summary { get; set; }
    }

    public static class WeatherSummaryModelExtenons
    {
        public static List<WeatherSummaryModel> ToList(this List<string> summaries)
        {
            var weatherSummaries = new List<WeatherSummaryModel>();

            foreach (var summary in summaries)
            {
                weatherSummaries.Add(new WeatherSummaryModel { Summary = summary });
            }

            return weatherSummaries;
        }
    }
}
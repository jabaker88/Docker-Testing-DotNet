using System.Text.Json;

namespace BlazorApp.Http
{
    public static class HttpUtility
    {
        private static string _baseUrl = string.Empty;
        private static string BaseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_baseUrl))
                {
                    var config = Config.Configuration?.Get<AppSettings>();
                    _baseUrl = config?.GatewayConfig?.GatewayAddress ?? string.Empty;
                }
                return _baseUrl;
            }
        }

        public static async Task<T?> GetFromGatewayAsync<T>(this HttpClient client, string path)
        {
            var response = await client.GetAsync(BaseUrl + FixPath(path));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                T? result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }

            return default;
        }

        public static async Task<string?> GetFromGatewayAsync(this HttpClient client, string path)
        {
            var response = await client.GetAsync(BaseUrl + FixPath(path));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return default;
        }

        //Appends "/" if missing from the path
        private static string FixPath(string path)
        {
            if (!path.StartsWith("/"))
            {
                return "/" + path;
            }

            return path;
        }
    }
}

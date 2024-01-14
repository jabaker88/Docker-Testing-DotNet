using System.Text;
using System.Text.Json;

namespace BlazorApp.Http
{
    public static class HttpUtility
    {
        public static async Task<T?> GetFromGatewayAsync<T>(this HttpClient client, string path)
        {
            var response = await client.GetAsync(FixPath(path));
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
            var response = await client.GetAsync(FixPath(path));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return default;
        }

        public static async Task<TResult?> PostToGatewayAsync<TArg,TResult>(this HttpClient client, string path, TArg data)
        {
            var response = await client.PostAsync(FixPath(path), new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                TResult? result = JsonSerializer.Deserialize<TResult>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
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

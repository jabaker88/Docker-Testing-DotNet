namespace BlazorApp
{
    public static class Config
    {
        public static void Init(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration? Configuration { get; private set; }
    }
}

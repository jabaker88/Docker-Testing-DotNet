namespace BlazorApp
{
    public class GateConfig
    {
        public string GatewayAddress { get; set; } = string.Empty;
    }
    public class AppSettings
    {
        public GateConfig GatewayConfig { get; set; } = new GateConfig();
    }
}

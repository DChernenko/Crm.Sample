namespace Crm.Sample.Infrastructure.Options
{
    public class SwaggerOptions
    {
        public bool Enabled { get; set; } = true;
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactUrl { get; set; }
        public string LicenseName { get; set; }
        public string LicenseUrl { get; set; }
        public bool EnableJwtSupport { get; set; } = false;
    }
}

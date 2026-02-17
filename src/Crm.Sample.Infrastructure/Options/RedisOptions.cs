namespace Crm.Sample.Infrastructure.Options
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public int DefaulExpirationInMinutes { get; set; }

    }
}

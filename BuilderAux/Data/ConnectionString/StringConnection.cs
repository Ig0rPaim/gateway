namespace BuilderAux.Data.ConnectionString
{
    public static class StringConnection
    {
        public static string GetString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            return configuration.GetConnectionString("DatabaseBuilderAux")
            ?? throw new NullReferenceException();
        }
    }
}

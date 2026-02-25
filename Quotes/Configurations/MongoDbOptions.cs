namespace Quotes.Configurations
{
    public class MongoDbOptions
    {
        // links appsettings.json section MongoDb to this class
        public const string SectionName = "MongoDb";

        public string ConnectionString { get; set; } = string.Empty;
        
        public string DatabaseName { get; set; } = string.Empty;
        
        public string SubscribersCollectionName { get; set; } = string.Empty;
    }
}

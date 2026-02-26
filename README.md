# Quotes
Good quotes.

# Use MongoDb

To use MongoDb set FeatureFlags: UseMongoDb in appsettings.json to true. If set to false InMemoryStarage will be used

# Setting up MongoDb container

1. Create local .env file in folder Infrastructure from pattern in .env.example

2. From Infrastructure folder run
   
   docker-compose up -d

   docker-compose down      (tear down container)
 
   docker-compose down -v   (tear down container AND cleanup data)
   
# Handeling MongoDb credentials in app

1. Set uo user secrets

   dotnet user-secrets init

   dotnet user-secrets set "MongoDb:ConnectionString"  "<your connectionstring with credentials>"

   dotnet user-secrets list  (check added credentials)

NOTE! if no user-secrets are set MongoDb will default to local instance ("mongodb://localhost:27017") 

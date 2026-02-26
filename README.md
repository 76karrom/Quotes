# Quotes
Good quotes.

# Run from powershell

$env:ASPNETCORE_ENVIRONMENT="Development"

    dotnet run

# Start MongoDB and Mongo Express containers in detached mode

From Infrastructure folder run

    docker-compose up -d

# Stop the containers

From Infrastructure folder run
    
    docker-compose down

# Stop and remove volumes (cleans up data)

From Infrastructure folder run

    docker-compose down -v

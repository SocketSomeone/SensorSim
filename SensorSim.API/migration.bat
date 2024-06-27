pause

dotnet ef migrations add Initial -c DataContext --project ../SensorSim.Domain.Migrations
dotnet ef database update -c DataContext --project ../SensorSim.Domain.Migrations
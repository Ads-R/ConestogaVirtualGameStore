# ConestogaVirtualGameStore
1. Dont forget to change the connection string in appsettings json
2. Install DNTCaptcha.Core(NuGet) latest version
## To Generate/Update the Database in SQL Server
### Open package manager console and enter the following commands
1. Add-Migration YourMigrationName -Context "GameStoreContext"
2. Update-Database -Context "GameStoreContext"

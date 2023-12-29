## Build and run instructions
Open Visual Studio Code and in the terminal navigate to the folder of the csproj. Ensure all dependencies are installed and run the command "dotnet run --roll-forward LatestMajor" to build the project and start the application. 
The database schema + data has been provided alongside the project. Use pg_restore to restore the database to a postgre sql server and change the connection string in appsettings.json as necessary. 
Default user email "dante@gmail.com" and password "danilo" can be used to access pages requiring authentication and authorization in the app.
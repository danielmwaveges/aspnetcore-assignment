
# QUEUE MANAGEMENT SYSTEM (Work in Progress)

A C# ASP.Net project intended to efficiently manage queues for various service oriented premises.



## Features

- Incorporate server side Blazor rendering for a smoother and interactive user experience.
- Real-time updates of queues and ticket status to waiting customers and service providers. (Done)
- Text to speech implementation of called customers (Done)
- Uses EFCore for a code first database modelling and migrations.
- Provides database context seed data for ease of testing and development.
- Reporting of served customers statistics using FastReport.Net (Done)
- BlazorBootstrap components for UI and styling. (Done)
- Uses Bcrypt algorithm for passwords hashing.
- Uses Humanizer package for readable date and time types.



## Run Locally
Ensure PostgreSql 15+ is installed and configure your connection string in appsettings.json


Clone the project

```bash
  git clone https://github.com/danielmwaveges/aspnetcore-assignment.git
```

Go to the project directory

```bash
  cd my-project/QueueManagementSystem.MVC
```

Apply migrations

```bash
  dotnet ef database update
```

Build and run the project locally. This command will install required package dependencies automatically.
```bash
  dotnet run
```


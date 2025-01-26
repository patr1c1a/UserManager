# User manager api (C# on .net)

## Prerequisites

- .NET SDK 9.0 or higher

## Dependencies

These dependencies will be restored automatically when you build the project:

- Microsoft.EntityFrameworkCore.Sqlite (for SQLite support)
- Microsoft.EntityFrameworkCore.Tools (for migrations)

Run the following command to restore dependencies:

```bash
dotnet restore
```

## Run server

```bash
dotnet run
```

By default it will run on [http://localhost:5111](http://localhost:5111).

## Migrations

Make sure dotnet-ef is installed:

```bash
dotnet ef --version
```

Or install by running:

```bash
dotnet tool install --global dotnet-ef
```

Run all migrations:

```bash
dotnet ef database update
```

## API Documentation

[http://localhost:5111/swagger](http://localhost:5111/swagger)

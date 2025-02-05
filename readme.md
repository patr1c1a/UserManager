# User manager api (C# on .net)

## Prerequisites

- .NET SDK 9.0 or higher
- SQLite

## Dependencies

The dependencies will be restored automatically when you build the project. Run the following command to restore dependencies:

```bash
dotnet restore
```

## Migrations

Make sure dotnet-ef is installed:

```bash
dotnet ef --version
```

Or install by running:

```bash
dotnet tool install --global dotnet-ef
```

Run all migrations before running server:

```bash
dotnet ef database update
```

## User-secrets for JWT

Generate a new key:

```bash
openssl rand -base64 32
```

Set up user-secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "generated_secret_key_here"
```

## Obtain a token

```bash
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin123!"
}
```

Please note that an "Admin" role and user with credentials admin/Admin123! and "Admin" role will be automatically created on start if a user or a role with those names are not found.

## Run server

```bash
dotnet run
```

By default it will run on [http://localhost:5111](http://localhost:5111).

## API Documentation

[http://localhost:5111/swagger](http://localhost:5111/swagger)

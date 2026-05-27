# ComputerManagerApi

REST API for managing computers and components — ASP.NET Core 8 + EF Core Code First.

## Requirements

- .NET 8 SDK
- SQL Server (LocalDB, Express, or full)

## Setup

### 1. Restore packages
```bash
dotnet restore
```

### 2. Configure the connection string

Edit `appsettings.json` → `ConnectionStrings:DefaultConnection`.  
Default points to SQL Server LocalDB:
```
Server=(localdb)\mssqllocaldb;Database=ComputerManagerDb;Trusted_Connection=True;
```

### 3. Apply the migration and seed data
```bash
dotnet ef database update
```
This creates the database, all tables, and inserts 3+ seed rows per table.

### 4. Run
```bash
dotnet run
```
Swagger UI is available at: `https://localhost:{port}/swagger`

---

## Endpoints

| Method | URL | Description | Success |
|--------|-----|-------------|---------|
| GET    | `/api/pcs` | List all computers | 200 |
| GET    | `/api/pcs/{id}/components` | Computer + its components | 200 / 404 |
| POST   | `/api/pcs` | Create a new computer | 201 |
| PUT    | `/api/pcs/{id}` | Update a computer | 200 / 404 |
| DELETE | `/api/pcs/{id}` | Delete computer + bindings | 204 / 404 |

---

## Project structure

```
ComputerManagerApi/
├── Controllers/
│   └── PcsController.cs        # HTTP layer — routes, status codes, no business logic
├── Data/
│   └── AppDbContext.cs          # DbContext, model config, HasData() seed
├── DTOs/
│   └── Dtos.cs                  # PcListItemDto, PcDetailDto, PcCreateDto, PcUpdateDto, …
├── Migrations/
│   ├── 20260508000000_InitialCreate.cs
│   ├── 20260508000000_InitialCreate.Designer.cs
│   └── AppDbContextModelSnapshot.cs
├── Models/
│   ├── Pc.cs
│   ├── Component.cs
│   ├── PcComponent.cs           # Junction table — composite PK (PcId, ComponentCode)
│   ├── ComponentType.cs
│   └── ComponentManufacturer.cs
├── Services/
│   ├── IPcService.cs            # Business logic interface
│   └── PcService.cs             # Implementation — all EF Core queries
├── Program.cs
├── appsettings.json
└── ComputerManagerApi.csproj
```

## Regenerating migrations (if you modify models)

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

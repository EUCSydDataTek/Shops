# Architecture

I denne øvelse bruger vi .Net 7.0+ til både app og class libraries.

## 1. Opret projekter
### opret følgende:

* __Webapp__ _(ASP.NET Core Web App)_
* __ServiceLayer__ _(Class Library)_
* __DataLayer__ _(Class Library)_

> _Du kan lave web projektet sammen med solutionen og oprette de andre herefter_

## 2. Installer Nuget Pakker

Der er to måder at installere nuget pakkerne på.
du vælger selv hvilen måde du vil bruge.

### Package Manager console (PMC) (Windows)
```pwsh
Install-Package Microsoft.EntityFrameworkCore.InMemory -ProjectName WebApp
Install-Package Microsoft.EntityFrameworkCore.SqlServer -ProjectName DataLayer
Install-Package Microsoft.EntityFrameworkCore.Tools -ProjectName DataLayer
```

### dotnet kommandoen (Windows,Linux,MacOS)
```bash
dotnet add WebApp package Microsoft.EntityFrameworkCore.InMemory
dotnet add DataLayer package Microsoft.EntityFrameworkCore.SqlServer
dotnet add DataLayer package Microsoft.EntityFrameworkCore.Tools
```

> _Hvis du bruger Visual studio 2022 eller højere kan du buge den visuelle brugeflade til at installere pakkerne i stedet._

## 3. Datalayer

Opret en klasse i datalayer med navn `AppDbContext.cs` med følgende indhold:

```C#
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {}
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {}

}
```

## 4. WebApp

Indsæt koden under linje 2 i `Program.cs`

```C#
services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
```

> Du skal muligvis lave nogle projekt referencer til ServiceLayer og DataLayer Da den skal bruge `AppDbContext` som dependency.

## 5. ServiceLayer

Skal være tom for nu.
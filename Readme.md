# 7. Swagger/OpenApi

I dette eksempel installerer vi swagger på vores api

> ℹ️ Der er en guide til hvordan man installerer swagger som er i [Microsoft docs](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-7.0)

## Nuget Pakker

Nuget pakken du skal installere hedder __Swashbuckle__ og skal installeres.

#### PMC:
```ps
Install-Package Swashbuckle.AspNetCore
```

## i `Program.cs`

### Sevices
Under services tilføjer du dette for at aktivere swagger generatoren.


```C#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer(); // 👈 Ny
builder.Services.AddSwaggerGen(); // 👈 Ny

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IShopService, ShopService>();
```

> ℹ️ Swagger generatoren er __ikke__ den som laver selve swagger siden men den som genererer en eller flere __xml__ filer som så swagger eller en andet program som for eksempel __Postman__ kan læse og lave et kort over apien.

> ℹ️ (Kort sagt) Swagger generatoren laver et kort over din API.


### Middleware pipeline

I din middleware tilføjer du swaggerUI som er siden der viser din API.

```C#
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()) // 👈 ny
{
    app.UseSwagger(); // 👈 ny
    app.UseSwaggerUI(); // 👈 ny
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

> ℹ️ Hvis du gerne vil have at siden bliver vist i produktion kan du fjerne if sætningen helt.

> ⚠️ I den virkelige verden vil man kun have swagger UI kørende under udvikling.

## Ændre `LaunchSettings.json` 

I dit WebApi projekt går du ind i mappen `Properties` og åbner `LaunchSettings.json`.

Ret derefter alle `launchUrl` fælternes value til __swagger__.

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:23256",
      "sslPort": 44303
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger", #👈 Ret her
      "applicationUrl": "http://localhost:5209",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger", #👈 Ret her
      "applicationUrl": "https://localhost:7289;http://localhost:5209",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger", #👈 Ret her
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## Test

Start Programmet.

Hvis du ser swagger komme op så er den sat korrekt op.
# 8. Sessions

I denne demo opgraderer vi in memory distributed cache med en Redis cache server.

Opret cache server med docker compose i roden af solutionen.
´´´pwsh
docker-compose up -d
´´´
> Docker compose kommer vi til senere.

## Webapp

installer redis nuget pakke
´´´pwsh
    Install-Package Microsoft.Extensions.Caching.StackExchangeRedis
´´´

### `Program.cs`

skift distributed memory cahche med redis chache
```
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379";
        options.InstanceName = "ShopCache";
    });
```


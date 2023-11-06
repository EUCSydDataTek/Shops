using DataLayer.Entities;
using Bogus;
using Bogus.Extensions;
using Bogus.DataSets;
using Xunit.Sdk;
namespace Unittest;

public static class Testhelpers
{
    public static List<Shop> CreateTestData(int amount)
    {
        Faker<Shop> shopfaker = new Faker<Shop>()
                                        .UseSeed(1234) // Presistant Seed data
                                        .RuleFor(s => s.ShopId,f => f.IndexGlobal)
                                        .RuleFor(s => s.Name,f => f.Company.CompanyName())
                                        .RuleFor(s => s.Location, f => f.Address.City())
                                        .RuleFor(s => s.Type, f => new ShopType() { ShopTypeId = f.Random.Number(), Name = f.Commerce.Product() });

        return shopfaker.Generate(amount);
    }
}

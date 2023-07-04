using DataLayer.Entities;
using System.Collections;
using System.Runtime.CompilerServices;
using WebApi.Models;

namespace WebApi.Mappers
{
    public static class ShopMapper
    {

        public static IEnumerable<ShopModel> MapToModel(this IEnumerable<Shop> shopModels)
        {
            return shopModels.Select(s => new ShopModel()
            {
                Name = s.Name,
                Location = s.Location,
                ShopId = s.ShopId,
                ShopType = s.Type?.Name ?? "",
                ShopTypeId = s.ShopTypeId
            }).ToList();
        }

        public static ShopModel MapToModel(this Shop shop)
        {
            return new ShopModel()
            {
                Name = shop.Name,
                Location = shop.Location,
                ShopId = shop.ShopId,
                ShopType = shop.Type?.Name ?? "",
                ShopTypeId = shop.ShopTypeId
            };
        }

        public static Shop MapToShop(this ShopCreateModel model)
        {
            return new Shop()
            {
                Name = model.Name,
                Location = model.Location,
                ShopTypeId = model.ShopTypeId,
            };
        }

        public static Shop MaptoShop(this ShopEditModel model)
        {
            return new Shop()
            {
                Name = model.Name,
                Location = model.Location,
                ShopTypeId = model.ShopTypeId,
                ShopId = model.ShopId,
            };
        }
    }
}

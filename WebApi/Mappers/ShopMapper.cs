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
                ShopType = s.Type.Name,
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
                ShopType = shop.Type.Name,
                ShopTypeId = shop.ShopTypeId
            };
        }

    }
}

using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {

        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        [HttpHead]
        public List<ShopModel> GetShops([FromQuery] SearchQueryModel searchQuery)
        {

            var model = _shopService.GetShopsByName(searchQuery.query,searchQuery.page,searchQuery.pageSize);

            // Metadata i headeren
            Response.Headers.Add("Page", searchQuery.page.ToString());
            Response.Headers.Add("PageSize", searchQuery.pageSize.ToString());
            Response.Headers.Add("TotalCount", model.TotalCount.ToString());

            return model.Shops.MapToModel().ToList();
        }

    }
}

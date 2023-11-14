using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class ShopsController : ControllerBase
    {

        private readonly IShopService _ShopService;

        public ShopsController(IShopService shopService)
        {
            _ShopService = shopService;
        }

        /// <summary>
        /// Henter En side med shops
        /// </summary>
        /// <param name="searchQuery">Søgning og paging af siden</param>
        /// <returns>En side med shops</returns>
        /// <response code="200">Har returneret shops</response> 
        [HttpGet]
        [HttpHead]
        [Produces("application/json", "text/oneline")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public List<ShopModel> GetShops([FromQuery] SearchQueryModel searchQuery)
        {

            var model = _ShopService.GetShopsByName(searchQuery.query,searchQuery.page,searchQuery.pageSize);

            // Metadata i headeren
            Response.Headers.Add("Page", searchQuery.page.ToString());
            Response.Headers.Add("PageSize", searchQuery.pageSize.ToString());
            Response.Headers.Add("TotalCount", model.TotalCount.ToString());

            return model.Shops.MapToModel().ToList();
        }

    }
}

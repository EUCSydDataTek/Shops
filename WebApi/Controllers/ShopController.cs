using Azure;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ServiceLayer;
using System.Reflection;
using System.Text;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly ILogger<ShopController> _Logger;

        public ShopController(IShopService shopService,ILogger<ShopController> logger)
        {
            _shopService = shopService;
            _Logger = logger;
        }

        /// <summary>
        /// Hent en shop med et specifikt ShopId
        /// </summary>
        /// <param name="shopId">ShopId'et fra den shop der skal hentes</param>
        /// <returns>Shoppen der tilhører shopId</returns>
        /// <response code="200">Returnerer shoppen som id'et tilhører</response>
        /// <response code="404">Shoppen findes ikke</response>
        [HttpGet(Name = "GetShop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetShop(int shopId)
        {

            Shop? shop = _shopService.GetShopById(shopId);

            if (shop != null)
            {
                var model = new ShopModel() {
                    ShopId = shopId,
                    Name = shop.Name,
                    ShopType = shop.Type.Name,
                    Location = shop.Location,
                    ShopTypeId = shop.ShopTypeId
                };

                return Ok(model);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Opretter en Shop
        /// </summary>
        /// <param name="model">Shoppen som skal oprettes</param>
        /// <returns>Returnerer den nyoprettede Shop</returns>
        /// <response code="201">Shoppen er blevet oprettet</response>
        /// <response code="422">Der skete en fejl under skriving til databasen</response>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Create(ShopCreateModel model)
        {
            var NewShop = model.MapToShop();

            try
            {
                _shopService.Add(NewShop);
                return CreatedAtAction("GetShop", new { shopId = NewShop.ShopId }, NewShop.MapToModel());
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Rediger en Shop
        /// </summary>
        /// <param name="model">Shoppen som skal redigeres</param>
        /// <returns>Returnerer den redigerede shop</returns>
        /// <response code="201">Shoppen er blevet redigeret</response>
        /// <response code="422">Der skete en fejl under skriving til databasen</response>
        [HttpPut]
        [Route("edit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Edit(ShopEditModel model) 
        {
            var shop = model.MaptoShop();

            try
            {
                _shopService.Update(shop);
                return CreatedAtAction("GetShop", new { shopId = shop.ShopId }, shop.MapToModel());
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }       
        }

        /// <summary>
        /// Redigerer delvist en Shop
        /// </summary>
        /// <param name="shopId">ShopId på shoppen som skal redigeres</param>
        /// <param name="patchDocument">Patch documentet som skal udføres på shoppen</param>
        /// <returns>Returnerer den redigerede shop</returns>
        /// <response code="201">Shoppen er blevet redigeret</response>
        /// <response code="404">Shoppen findes ikke</response>
        /// <response code="422">Der skete en fejl under skriving til databasen</response>
        [HttpPatch]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult EditPartially(int shopId,[FromBody] JsonPatchDocument<ShopModel> patchDocument)
        {
            Shop? shop = _shopService.GetShopById(shopId);

            if (shop == null)
            {
                return NotFound();
            }

            LogPatchDoc(patchDocument);

            ShopModel model = shop.MapToModel();

            try
            {

                patchDocument.ApplyTo(model);

                shop.Location = model.Location;
                shop.Name = model.Name;
                shop.ShopTypeId = model.ShopTypeId;

                _shopService.Update(shop);

            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
                return UnprocessableEntity(e.Message);
            }

            return CreatedAtAction("GetShop", new { shopId = model.ShopId },model);
        }

        /// <summary>
        /// Fjerner en shop
        /// </summary>
        /// <param name="Shopid">ShopId til shoppen som skal fjernes</param>
        /// <returns>Ingenting</returns>
        /// <response code="204">Shoppen blev slettet</response>
        /// <response code="404">Shoppen findes ikke</response>
        /// <response code="422">Der skete en fejl under skriving til databasen</response>
        [HttpDelete]
        [Route("remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Remove(int Shopid)
        {
            var shop = _shopService.GetShopById(Shopid);

            if (shop == null)
                return NotFound();

            try
            {
                bool result = _shopService.Delete(Shopid);

                if (result)
                {
                    return NoContent(); // Success
                }
                else
                {
                    return UnprocessableEntity(); // fejl under delete
                }
            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
                return UnprocessableEntity(e.Message);
            }
        }

        private void LogPatchDoc<T>(JsonPatchDocument<T> document) where T : class
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\n----JsonPatch DEBUG----");

            foreach (var op in document.Operations) {
                sb.Append($"\n-- \n Operation: {op.op}");
                sb.Append($"\n Path: {op.path}");

                switch (op.OperationType)
                {
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Add:
                        sb.Append($"\n Value: {op.value}");
                        break;
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Remove:
                        break;
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Replace:
                        sb.Append($"\n Value: {op.value}");
                        break;
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Move:
                        sb.Append($"\n From: {op.from}");
                        break;
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Copy:
                        sb.Append($"\n From: {op.from}");
                        break;
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Test:
                        sb.Append($"\n Value: {op.value}");
                        break;
                    case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Invalid:
                        break;
                }

                _Logger.LogInformation( sb.ToString() );

            }
        }

    }
}



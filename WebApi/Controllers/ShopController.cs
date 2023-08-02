using Azure;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ServiceLayer;
using System.Text;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet(Name = "GetShop")]
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

        [HttpPost]
        [Route("create")]
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

        [HttpPut]
        [Route("edit")]
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

        [HttpPatch]
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



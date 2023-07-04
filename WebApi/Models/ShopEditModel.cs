using DataLayer.Entities;

namespace WebApi.Models
{
    public class ShopEditModel
    {
        public int ShopId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ShopTypeId { get; set; }

        public string Location { get; set; } = string.Empty;
    }
}

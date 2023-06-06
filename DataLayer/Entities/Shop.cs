using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Shop
    {
        public int ShopId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ShopTypeId { get; set; }

        public string Location { get; set; } = string.Empty;

        public ShopType Type { get; set; } = default!;

    }
}

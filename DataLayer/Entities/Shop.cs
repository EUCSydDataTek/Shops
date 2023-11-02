using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Shop
    {
        public int ShopId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageMimeType { get; set; } = string.Empty;

        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[] ImageData { get; set; } = new byte[0];

        public int ShopTypeId { get; set; }

        public string Location { get; set; } = string.Empty;

        public ShopType Type { get; set; } = default!;

    }
}

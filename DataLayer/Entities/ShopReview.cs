using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class ShopReview
    {
        public int ShopReviewId { get; set; }

        public string subject { get; set; } = default!;

        public string text { get; set; } = default!;

        public short Stars { get; set; }
    }
}

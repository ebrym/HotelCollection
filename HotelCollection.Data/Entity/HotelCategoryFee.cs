using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Data.Entity
{
    public class HotelCategoryFee : BaseEntity.Entity
    {
        public double Fee { get; set; }
        public virtual HotelCategory Category { get; set; }
        public int CategoryId { get; set; }
    }
}

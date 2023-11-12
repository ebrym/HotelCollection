using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Data.Entity
{
    public class Hotel : BaseEntity.Entity
    {
            public string Name  { get; set; }
            public string Address  { get; set; }
            public string Phone  { get; set; }
            public string Email  { get; set; }
            public string CACNumber  { get; set; }
            public virtual HotelCategory Category  { get; set; }
            public int CategoryId  { get; set; }
            public virtual LocalGovernmentArea LocalGovernmentArea { get; set; }
            public int LGAId { get; set; }
    }
}

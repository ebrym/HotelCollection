using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Data.Entity
{
   public class RequisitionDetails : BaseEntity.Entity
    {
        public int ItemCategoryId { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public int QuantityIssued { get; set; }
        public int QuantityToProcure { get; set; }
        public bool IsProcurement { get; set; } = false;
        public bool Issued { get; set; }
        public string IssuedBy { get; set; }
        public DateTime DateIssued { get; set; }
    }
}

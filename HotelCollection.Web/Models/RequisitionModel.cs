using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Web.Models
{
    public class RequisitionModel
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
        public string StaffEmail { get; set; }
        public string StaffNumber { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string JobTitle { get; set; }
        public string LineManager { get; set; }
        public string RequisitionType { get; set; }
        public string ApprovalStatus { get; set; }
        public string CurrentApprovalLevel { get; set; }
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string MarketerName { get; set; }
        public string MarketerEmail { get; set; }
        public string Remarks { get; set; }
        public IFormFile UploadDoc { get; set; }
        public DateTime DateCreated { get; set; }
        public string DocumentPath { get; set; }
        public string ApprovalComments { get; set; }
        public string Status { get; set; }
        public List<ItemsModel> Items { get; set; }
        public string AppLink { get; set; }
        public bool Approved { get; set; }


    }

    public class ItemsModel
    {
        public int Id { get; set; }
        public int HotelCategoryId { get; set; }
        public string HotelCategory { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public int QuantityIssued { get; set; }
        public int QuantityToProcure { get; set; }
        public bool IsProcurement { get; set; }
        public bool Issued { get; set; }
        public string IssuedBy { get; set; }
        public DateTime DateIssued { get; set; }
    }
}

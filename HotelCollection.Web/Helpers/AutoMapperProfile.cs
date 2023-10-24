using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelCollection.Web.Models;
using HotelCollection.Data.Entity;

namespace HotelCollection.Web.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ItemCategoryModel, ItemCategory>().ReverseMap();
            CreateMap<Requisition, RequisitionModel>()
                .ForMember(pts => pts.Items, opt => opt.MapFrom(ps => ps.RequisitionDetails))
                .ReverseMap();
            CreateMap<RequisitionModel, Requisition>()
                .ReverseMap();

            CreateMap<RequisitionDetails, ItemsModel>().ReverseMap();
            CreateMap<ItemsModel, RequisitionDetails>().ReverseMap();

            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
            CreateMap<RegisterViewModel, ApplicationUser>().ReverseMap();
            CreateMap<RoleViewModel, Role>().ReverseMap();

            CreateMap<ApprovalConfigModel, ApprovalConfig>().ReverseMap();

            //CreateMap<List<RequisitionDetails>, List<ItemsModel>>().ReverseMap();
            //CreateMap<List<ItemsModel>, List<RequisitionDetails>>().ReverseMap();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.ReportRepo
{
    public class ReportRepository : IReportRepo
    {
        private readonly HotelCollectionContext context;

        public ReportRepository(HotelCollectionContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Requisition>> GetAll()
        {
            return await context.Requisitions.OrderByDescending(x => x.DateCreated)
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<Requisition>> GetByApprovalStatus(string approvalStatus, DateTime startDate, DateTime endDate)
        {
            return await context.Requisitions.Where(x => x.ApprovalStatus == approvalStatus &&
                                                        x.DateCreated.Date >= startDate.Date &&
                                                        x.DateCreated.Date < endDate.AddDays(1).Date
                                                   )
                                                   .OrderByDescending(x => x.DateCreated)
                                                   .ToListAsync();
        }

        public async Task<IEnumerable<Requisition>> GetByDepartmentRequisitionTypeAndDate(string department, string requisitionType, DateTime startDate, DateTime endDate)
        {
            return await context.Requisitions.Where(x => x.Department == department && 
                                                         x.RequisitionType == requisitionType &&
                                                         x.DateCreated.Date >= startDate.Date && 
                                                         x.DateCreated.Date < endDate.AddDays(1).Date
                                                    )
                                                    .OrderByDescending(x => x.DateCreated)
                                                    .ToListAsync();
        }

        public async Task<IEnumerable<Requisition>> GetByDisbursedStatus(string disburseStatus, DateTime startDate, DateTime endDate)
        {
           var requisitions = await context.Requisitions.Where(x => x.Status == disburseStatus && 
                                                               x.DateCreated.Date >= startDate.Date &&
                                                         x.DateCreated.Date < endDate.AddDays(1).Date
                                                    )
                                                   .OrderByDescending(x => x.DateCreated)
                                                   .ToListAsync();
            return requisitions;
        }

        public async Task<IEnumerable<Requisition>> GetByProject(int projectId, DateTime startDate, DateTime endDate)
        {
            return await context.Requisitions.Where(x => x.ProjectId == projectId &&
                                                         x.DateCreated.Date >= startDate.Date &&
                                                         x.DateCreated.Date < endDate.AddDays(1).Date
                                                    )
                                                    .OrderByDescending(x => x.DateCreated)
                                                    .ToListAsync();
        }
        public async Task<IEnumerable<Hotel>> GetAllHotels()
        {
            return await context.Hotels.OrderByDescending(x => x.DateCreated)
                                             .Include(x => x.Category)
                                            .Include(x => x.LocalGovernmentArea)
                                            .ToListAsync();
        }
        public async Task<IEnumerable<Hotel>> GetHotelsByCategory(int categoryid)
        {
            return await context.Hotels.Where(x => x.CategoryId == categoryid 
                                                    )
                                                     .Include(x => x.Category)
                                                     .Include(x => x.LocalGovernmentArea)
                                                    .ToListAsync();
        }
        public async Task<IEnumerable<Hotel>> GetByHotelsByLGA(int lgaId)
        {
            return await context.Hotels.Where(x => x.LGAId == lgaId)
                                                    .Include(x => x.Category)
                                                     .Include(x => x.LocalGovernmentArea)
                                                    .OrderByDescending(x => x.DateCreated)
                                                    .ToListAsync();
        }
    }
}

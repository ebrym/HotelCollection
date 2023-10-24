
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HotelCollection.Repository.AccountRepo;
using HotelCollection.Repository.ApprovalRepo;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.Repo
{
   public class RequisitionRepository : IRequisition
    {
        private readonly HotelCollectionContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IItemCategoryRepository _itemCategoryRepository;
        private readonly IAccountManager _accountManager;
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;

        public RequisitionRepository(HotelCollectionContext context,
                                     IItemCategoryRepository itemCategoryRepository,
                                     IHttpContextAccessor contextAccessor, 
                                     IApprovalConfigRepository ApprovalConfigRepository,
                                     IAccountManager accountManager)
        {
            _itemCategoryRepository = itemCategoryRepository;           
            _contextAccessor = contextAccessor;           
            _accountManager = accountManager;
            _context = context;
            _ApprovalConfigRepository = ApprovalConfigRepository;
    }


    public async Task<bool> CreateRequisitionAsync(Requisition requisition)
       {
            if (requisition != null)
            {
                //requisition.CurrentApprovalLevel = "1";
                await _context.AddAsync(requisition);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

       public async Task<IEnumerable<Requisition>> GetRequisitionsAsync()
        {
            return await _context.Requisitions.ToListAsync();
        }

        public async Task<IEnumerable<Requisition>> GetRequisitionsByUserAsync()
        {
            var currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userReq = await _context.Requisitions.Where(x => x.CreatedBy == currentUser && x.IsDeleted==false)
                                                        .Include(x => x.RequisitionDetails)      
                                                        .OrderByDescending(x=>x.DateCreated)
                                                        .Take(10)
                                                        .ToListAsync();


            return userReq;
        }

        public async Task<IEnumerable<Requisition>> GetRequisitionsDashBoardByUserAsync()
        {
            var currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userReq = await _context.Requisitions.Where(x => x.CreatedBy == currentUser && x.IsDeleted == false)
                                                        .ToListAsync();


            return userReq;
        }
        public async Task<Requisition> GetRequisitionByIdAsync(int Id)
        {
            return await _context.Requisitions.FindAsync(Id);
        }

        public async Task<Requisition> GetRequisitionDetailsAsync(int Id)
        {
           var reqDetails = await _context.Requisitions.Where(x => x.Id == Id && x.IsDeleted == false).Include(x => x.RequisitionDetails).FirstOrDefaultAsync();
           // var reqDetails = await _context.Requisitions.Where(x => x.Id == Id).FirstOrDefaultAsync();
            return reqDetails;
        }

        public async Task<bool>UpdateRequisitionAsync(Requisition requisition)
        {
            if (requisition != null)
            {
                _context.Update(requisition);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

       public async Task<bool> DeleteRequisitionAsync(int Id)
        {
            var category = await _context.ItemCategories.Where(x => x.Id == Id).FirstAsync();
            if (category != null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetCurrentApprovalLevel(int Id)
        {
            var  req =  await _context.Requisitions.FindAsync(Id);

            if (req == null)
                return 0;

            return Convert.ToInt16(req.CurrentApprovalLevel);
      
        }

        public async Task<Requisition> GetRequisitionDetailsByUserAsync(int Id)
        {
            
            //get current logged on user
            var currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            
          

            var department = _contextAccessor.HttpContext.User.FindFirst("department").Value;
            var unit = _contextAccessor.HttpContext.User.FindFirst("unit").Value;
            Requisition userReq = new Requisition();

                userReq = await _context.Requisitions.Where(x => x.Id == Id &&
                                                                    x.CreatedBy == currentUser &&
                                                                    x.IsDeleted == false &&
                                                                    x.Department == department)
                                                     .Include(x => x.RequisitionDetails)
                                                     .FirstOrDefaultAsync();
           
            return userReq;
        }
    }
}

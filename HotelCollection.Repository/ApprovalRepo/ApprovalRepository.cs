
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Numerics;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.AccountRepo;
using HotelCollection.Repository.Dtos;

namespace HotelCollection.Repository.ApprovalRepo
{
   public class ApprovalRepository : IApprovalRepository
    {
        private readonly HotelCollectionContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAccountManager _accountManager;
        private readonly IApprovalConfigRepository _approvalConfig;
        private IConfiguration _config;

        public ApprovalRepository(HotelCollectionContext context,
                                     IAccountManager accountManager, IConfiguration config, 
                                     IHttpContextAccessor contextAccessor,
                                     IApprovalConfigRepository approvalConfig)
        {
            _context = context;
            _config = config;
            _contextAccessor = contextAccessor;
            _accountManager = accountManager;
            _approvalConfig = approvalConfig;
        }

        public async Task<bool> CreateApprovalAsync(PaymentSetup approval, string remarks, bool isApproved)
        {
            try
            {
                var approvaleUser = _contextAccessor.HttpContext.User.FindFirst("Email").Value;
                //var approvalRole = _contextAccessor.HttpContext.User.FindFirst("mail").Value;
                var approvalLevel = _contextAccessor.HttpContext.User.FindFirst("UserApprovalLevel").Value;
                var isFinalApproval = await _approvalConfig.IsFinalApprovalsAsync(approval.Id);//_contextAccessor.HttpContext.User.FindFirst("IsFinalApproval").Value;

                var request = await _context.PaymentSetups.Where(x => x.Id == approval.Id).FirstOrDefaultAsync();
                using (var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted))
                {


                    Approval aprv = new Approval
                    {
                        Comment = remarks,
                        ApproverUserId = await _accountManager.GetUserByEmailAsync(approvaleUser),
                        PaymentId= request.Id,
                        ApprovalLevel= Convert.ToInt16(request.CurrentApprovalLevel)

                    };
                    await _context.AddAsync(aprv);

                    // check if approved or rejected
                    if (isApproved)
                    {
                        // increment if not final approval
                        if (!isFinalApproval)
                            request.CurrentApprovalLevel = (Convert.ToInt16(request.CurrentApprovalLevel) + 1).ToString();
                        else
                            request.ApprovalStatus = "Completed";
                    }
                    else
                    {
                        request.ApprovalStatus = "Rejected";
                    }

                   

                    _context.Update<PaymentSetup>(request);

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }           
            
        }

        public async Task<PaymentSetup> GetPaymentApprovalDetailsAsync(int Id)
        {
           // var reqDetails = await _context.Requisitions.Where(x => x.Id == Id && x.IsDeleted == false).Include(x => x.RequisitionDetails).FirstOrDefaultAsync();

            //get current logged on user
            var currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var userApprovalLevel = _contextAccessor.HttpContext.User.FindFirst("UserApprovalLevel").Value;
            PaymentSetup userReq = new PaymentSetup();

            if (userApprovalLevel =="1")
            {

                 userReq = await _context.PaymentSetups.Where(x => x.Id == Id  && 
                                                             x.CurrentApprovalLevel == userApprovalLevel.ToString() && 
                                                             x.IsDeleted == false &&
                                                             x.ApprovalStatus == null)
                     .Include(x=> x.Hotel)
                     .ThenInclude(x=>x.LocalGovernmentArea)
                     .Include(x=> x.Hotel)
                     .ThenInclude(x=>x.Category)
                     .Include(x=> x.PaymentType)
                                                         .FirstOrDefaultAsync();

            }
            else
            {
                 userReq = await _context.PaymentSetups.Where(x => x.Id == Id && 
                                                                   x.CurrentApprovalLevel == userApprovalLevel.ToString() && 
                                                                   x.IsDeleted == false && 
                                                                   x.ApprovalStatus == null)
                     .Include(x=> x.Hotel)
                     .ThenInclude(x=>x.LocalGovernmentArea)
                     .Include(x=> x.Hotel)
                     .ThenInclude(x=>x.Category)
                     .Include(x=> x.PaymentType)
                                                      .FirstOrDefaultAsync();
            }
            return userReq;
        }


      
        public async Task<IEnumerable<PaymentSetup>> GetPendingApprovalAsync()
        {

            //get current logged on user
            var currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userApprovalLevel = _contextAccessor.HttpContext.User.FindFirst("UserApprovalLevel").Value;
            var isFinalApproval = _contextAccessor.HttpContext.User.FindFirst("IsFinalApproval").Value;
      
            List<PaymentSetup> userReq = new List<PaymentSetup>();

            // for unit level approval 1
            if (userApprovalLevel == "1")
            {

                userReq = await _context.PaymentSetups.Where(x => x.CurrentApprovalLevel == userApprovalLevel.ToString() && 
                                                                 x.IsDeleted == false && 
                                                                 x.ApprovalStatus == null)
                                                         .OrderByDescending(x => x.DateCreated)
                                                         .Include(x=>x.Hotel)
                                                            .ThenInclude(x=>x.Category)
                                                         .Include(x=>x.PaymentType)
                                                         .ToListAsync();            }
            else
            {
                // for other level
                if (Convert.ToBoolean(isFinalApproval))
                    userReq = await _context.PaymentSetups.Where(x => x.CurrentApprovalLevel == userApprovalLevel.ToString() && 
                                                                     x.IsDeleted == false &&
                                                                     x.ApprovalStatus == null)
                                                          .OrderByDescending(x => x.DateCreated)
                                                          .Include(x=>x.Hotel)
                                                          .ThenInclude(x=>x.Category)
                                                          .Include(x=>x.PaymentType)
                                                          .ToListAsync();
                else
                    userReq = await _context.PaymentSetups.Where(x => x.CurrentApprovalLevel == userApprovalLevel.ToString() && 
                                                                     x.IsDeleted == false &&
                                                                     x.ApprovalStatus == null)
                                                           .OrderByDescending(x => x.DateCreated)
                                                           .Include(x=>x.Hotel)
                                                           .ThenInclude(x=>x.Category)
                                                           .Include(x=>x.PaymentType)
                                                           .ToListAsync();

            }
            return userReq;
        }
        public async Task<IEnumerable<Approval>> GetApprovalCommentsAsync(int requestId)
        {
            var comments = await _context.Approvals.Where(x => x.PaymentId == requestId)
                                        .OrderByDescending(x=>x.DateCreated)
                                        .Include(x=>x.ApproverUserId)
                                        .ToListAsync();
              
            return comments;
        }

        //public async  Task<IEnumerable<Requisition>> GetApprovedRequestsAsync()
        //{
        //   var userReq = await _context.Requisitions.Where(x =>  x.ApprovalStatus != null)
        //                                                .Include(x => x.RequisitionDetails)
        //                                                .OrderByDescending(x => x.DateCreated)
        //                                                .ToListAsync();
        //    return userReq;
        //}

        public async Task<IEnumerable<PaymentSetup>> GetApprovedRequestsAsync()
        {
            var userReq = await _context.PaymentSetups.Where(x => x.ApprovalStatus != null && 
                                                                  x.ApprovalStatus != "Complete")
                                                         .OrderByDescending(x => x.DateCreated)
                                                         .Include(x=>x.Hotel)
                                                         .ThenInclude(x=>x.Category)
                                                         .Include(x=>x.PaymentType)
                                                         .ToListAsync();
            return userReq;
        }

        public async Task<PaymentSetup> GetPaymentDetailsAsync(int Id)
        {
           
            PaymentSetup userReq = new PaymentSetup();

          

                userReq = await _context.PaymentSetups.Where(x => x.Id == Id &&
                                                                 x.ApprovalStatus != null)
                                                        .FirstOrDefaultAsync();

           
            return userReq;
        }

        public async Task<bool> DisburseRequest(string Status, int requestId, int[] Id, decimal[] QuantityIssued, bool[] IsProcurement)
        {
            try
            {
                var currentUser = _contextAccessor.HttpContext.User.Identity.Name;

                var Request = await _context.Requisitions.Where(x => x.Id == requestId)
                                                            .Include(x => x.RequisitionDetails).FirstOrDefaultAsync();
                for (int i = 0; i < QuantityIssued.Length; i++)
                {

                    var oldEntry = Request.RequisitionDetails.Where(x => x.HotelCategoryId == Id[i]).FirstOrDefault();
                    if (oldEntry == null)
                    {
                        return false;
                    }

                    oldEntry.QuantityIssued = Convert.ToInt16(QuantityIssued[i]);
                    oldEntry.IsProcurement = IsProcurement[i];
                    if (IsProcurement[i])
                    {
                        oldEntry.QuantityToProcure = oldEntry.Quantity - oldEntry.QuantityIssued;
                    }
                    oldEntry.IssuedBy = currentUser;
                    oldEntry.DateIssued = DateTime.Now;
                }
                Request.Status = Status;
                //Request.
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }
}

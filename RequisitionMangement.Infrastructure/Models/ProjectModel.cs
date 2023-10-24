using System;
using System.Collections.Generic;
using System.Text;

namespace HotelCollection.Infrastructure.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public AccountManager accountManager { get; set; }

    }

    public class AccountManager
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string jobTitle { get; set; }
        public string department { get; set; }
        public string userName { get; set; }
        public string email { get; set; }



    }
    public class ProjectMarketer
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string MarketerName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }



    }
}

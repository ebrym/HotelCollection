using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Infrastructure.Models;

namespace HotelCollection.Infrastructure.Interface
{
  public interface IProjectService : IDependencyRegister
    {
        Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
    }
}

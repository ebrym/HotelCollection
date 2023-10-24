using HotelCollection.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCollection.Infrastructure.Interface
{
    public interface IViewRenderService : IDependencyRegister
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}

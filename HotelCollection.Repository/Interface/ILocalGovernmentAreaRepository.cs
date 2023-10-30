using HotelCollection.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelCollection.Repository.Interface
{
    public interface ILocalGovernmentAreaRepository
    {
        Task<bool> CreateLocalGovernmentAreaAsync(LocalGovernmentArea lga);
        Task<IEnumerable<LocalGovernmentArea>> GetLocalGovernmentAreaAsync();
        Task<LocalGovernmentArea> GetLocalGovernmentAreaByIdAsync(int Id);
        Task<bool> UpdateLocalGovernmentAreaAsync(LocalGovernmentArea lga);
        Task<bool> DeleteLocalGovernmentAreaAsync(int Id);
    }
}

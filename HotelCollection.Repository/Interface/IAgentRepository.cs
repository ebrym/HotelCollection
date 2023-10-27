using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCollection.Data.Entity;

namespace HotelCollection.Repository.Interface;

public interface IAgentRepository
{
    Task<bool> CreateAgentAsync(Agent agent);
    Task<IEnumerable<Agent>> GetAgentAsync();
    Task<Agent> GetAgentByIdAsync(int Id);
    Task<bool> UpdateAgentAsync(Agent agent);
    Task<bool> DeleteAgentAsync(int Id);
}
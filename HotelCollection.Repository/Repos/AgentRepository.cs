
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Repository.Interface;

namespace HotelCollection.Repository.Repo
{
   public class AgentRepository : IAgentRepository
    {
        private readonly HotelCollectionContext _context;

        public AgentRepository(HotelCollectionContext context )
        {
            _context = context;
        }

        public async Task<bool> CreateAgentAsync(Agent agent)
        {
            if (agent != null)
            {

                await _context.AddAsync(agent);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Agent>> GetAgentAsync()
        {
            return await _context.Agents.ToListAsync();
        }

        public async Task<Agent> GetAgentByIdAsync(int Id)
        {
            return await _context.Agents.FindAsync(Id);
        }

        public async Task<bool> UpdateAgentAsync(Agent agent)
        {
            if (agent != null)
            {
                _context.Update(agent);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAgentAsync(int Id)
        {
            var category = await _context.Agents.Where(x=>x.Id==Id).FirstAsync();
            if (category != null)
            {
                 _context.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}

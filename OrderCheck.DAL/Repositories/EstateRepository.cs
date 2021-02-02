using Microsoft.EntityFrameworkCore;
using OrderCheck.DAL.Contexts;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCheck.DAL.Repositories
{
    public class EstateRepository : BaseRepository<Estate>, IEstateRepository
    {
        private readonly OrderCheckContext _context;

        public EstateRepository(OrderCheckContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Estate>> EstatesByOwnerIdAsync(string ownerId)
        {
            return await _context.Estates
                .Where(e => e.OwnerId.Equals(ownerId))
                .ToListAsync();
        }
    }
}

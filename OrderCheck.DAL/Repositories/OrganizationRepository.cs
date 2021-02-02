using Microsoft.EntityFrameworkCore;
using OrderCheck.DAL.Contexts;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCheck.DAL.Repositories
{
    public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
    {
        private readonly OrderCheckContext _context;

        public OrganizationRepository(OrderCheckContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Organization>> OrganizationsByOwnerIdAsync(string ownerId)
        {
            return await _context.Organizations
                .Where(e => e.OwnerId.Equals(ownerId))
                .ToListAsync();
        }
    }
}

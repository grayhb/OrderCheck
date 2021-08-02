using Microsoft.EntityFrameworkCore;
using OrderCheck.DAL.Contexts;
using OrderCheck.DAL.Interfaces;
using OrderCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCheck.DAL.Repositories
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        private readonly OrderCheckContext _context;

        public DocumentRepository(OrderCheckContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Document> DocumentByGuidAsync(Guid guid)
        {
            return await _context.Documents
                .Include(e => e.Organization)
                .Include(e => e.Estate)
                .SingleOrDefaultAsync(e => !e.Deleted && e.Guid == guid);
        }

        public async Task<List<Document>> DocumentsByOwnerIdAsync(string ownerId)
        {
            return await _context.Documents
                .Include(e => e.Organization)
                .Include(e => e.Estate)
                .Where(e => !e.Deleted && e.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task RemoveAsync(Document item)
        {
            item.Deleted = true;
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}

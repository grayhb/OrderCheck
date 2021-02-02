using OrderCheck.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCheck.DAL.Interfaces
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task<List<Document>> DocumentsByOwnerIdAsync(string ownerId);
        Task<Document> DocumentByGuidAsync(Guid guid);
    }
}

using OrderCheck.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCheck.DAL.Interfaces
{
    public interface IOrganizationRepository : IBaseRepository<Organization>
    {
        Task<List<Organization>> OrganizationsByOwnerIdAsync(string ownerId);
    }
}

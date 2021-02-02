using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCheck.DAL.ViewModels;
using OrderCheck.Models;

namespace OrderCheck.DAL.Interfaces
{
    public interface IEstateRepository : IBaseRepository<Estate>
    {
        Task<List<Estate>> EstatesByOwnerIdAsync(string ownerId);
    }
}

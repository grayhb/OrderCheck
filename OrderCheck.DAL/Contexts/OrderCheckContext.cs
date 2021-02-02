using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderCheck.Models;

namespace OrderCheck.DAL.Contexts
{
    public class OrderCheckContext :  IdentityDbContext<User>
    {
        public OrderCheckContext(DbContextOptions<OrderCheckContext> options) : base(options) { }

        public virtual DbSet<Estate> Estates { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
    }
}

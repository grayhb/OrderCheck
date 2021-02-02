using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderCheck.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }
}

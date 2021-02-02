using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderCheck.Models
{
    [Table("Organizations")]
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        [JsonIgnore]
        public string OwnerId { get; set; }

        [JsonIgnore]
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
    }
}

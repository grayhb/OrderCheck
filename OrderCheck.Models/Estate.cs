using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderCheck.Models
{
    [Table("Estates")]
    public class Estate
    {
        [Key]
        public int EstateId { get; set; }
        public string EstateName { get; set; }
        public string EstateAddress { get; set; }

        [JsonIgnore]
        public string OwnerId { get; set; }

        [JsonIgnore]
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderCheck.Models
{
    [Table("Documents")]
    public class Document
    {
        [JsonIgnore]
        [Key]
        public long DocumentId { get; set; }

        public Guid Guid { get; set; }


        [JsonIgnore]
        public string ImagePath { get; set; }

        [JsonIgnore]
        public string CheckImagePath { get; set; }

        [DefaultValue(0)]
        public decimal Debt { get; set; }

        [DefaultValue(0)]
        public decimal Paid { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public DateTime Created { get; set; }
       
        public string Note { get; set; }


        [JsonIgnore]
        public string OwnerId { get; set; }

        public int OrganizationId { get; set; }
        public int EstateId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        [ForeignKey("EstateId")]
        public Estate Estate { get; set; }

        public bool Deleted { get; set; }

        public string QrInfo { get; set; }

        [NotMapped]
        public bool HaveCheck
        {
            get { return !string.IsNullOrEmpty(CheckImagePath); }
        }
    }
}

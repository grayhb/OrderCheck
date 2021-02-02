using System;

namespace OrderCheck.Web.ViewModels
{
    public class DocumentViewModel
    {
        public Guid Guid { get; set; }
        public decimal Debt { get; set; }
        public decimal Paid { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Note { get; set; }

        public int OrganizationId { get; set; }
        public int EstateId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VicUniIndustryProject2025LiveKit.EditableModels
{
    public class Visitor
    {

        public Visitor(string Name, DateTime ArrivalTime, bool IsOnSite, string Reason, string? MeetingWith, string? ContractorCompany, DateTime? DepartureTime)
        {
            this.Name = Name;
            this.ArrivalTime = ArrivalTime;
            this.IsOnSite = IsOnSite;
            this.Reason = Reason;
            this.MeetingWith = MeetingWith;
            this.ContractorCompany = ContractorCompany;
            this.DepartureTime = DepartureTime;
        }

        [Key] // Ensures it's the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures auto-increment behavior
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool IsOnSite { get; set; }
        public string Reason { get; set; }
        public string? MeetingWith { get; set; }
        public string? ContractorCompany { get; set; }
        public DateTime? DepartureTime { get; set; }
    }
}

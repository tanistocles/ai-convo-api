namespace VicUniIndustryProject2025LiveKit
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

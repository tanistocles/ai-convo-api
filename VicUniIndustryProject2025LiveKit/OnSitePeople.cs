namespace VicUniIndustryProject2025LiveKit
{
    public class OnSitePeople
    {
        public OnSitePeople(int Id, string Name, string Type) {
            this.Id = Id;
            this.Name = Name;
            this.Type = Type;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}

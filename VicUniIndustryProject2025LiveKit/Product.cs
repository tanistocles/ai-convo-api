using Microsoft.Identity.Client;

namespace VicUniIndustryProject2025LiveKit
{
    public class Product
    {
        public Product(int id, string productName, string productDescription)
        {
            this.id = id;
            this.productName = productName;
            this.productDescription = productDescription;
        }
        public int id { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }


    }
}
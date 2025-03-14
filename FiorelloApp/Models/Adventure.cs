namespace FiorelloApp.Models
{
    public class Adventure:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public  int  Price { get; set; }
        public int AdventureCount { get; set; }
        public string Image { get; set; }
    }
}

namespace LADP__EFC.Models
{
    public class ResourceTags
    {
        public int TagId { get; set; }
        public int FoodResourceId { get; set; }
        public Tag Tag { get; set; } = null!; // Many-to-One relationship with Tag
        public FoodResource FoodResource { get; set; } = null!; // Many-to-One relationship with FoodResource
    }
}

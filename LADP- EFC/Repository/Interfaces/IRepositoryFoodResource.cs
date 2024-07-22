using LADP__EFC.Models;

namespace LADP__EFC.Repository.Interfaces
{
    public interface IRepositoryFoodResource
    {
        IEnumerable<FoodResource> GetFoodResources();
        FoodResource GetFoodResource(int id);
        FoodResource PutFoodResource(int id, FoodResource foodResource);
        FoodResource PostFoodResource(FoodResource foodResource);
        FoodResource DeleteFoodResource(int id);
    }
}

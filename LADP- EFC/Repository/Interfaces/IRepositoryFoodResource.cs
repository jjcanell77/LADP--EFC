using LADP__EFC.DTO;
using LADP__EFC.Models;

namespace LADP__EFC.Repository.Interfaces
{
    public interface IRepositoryFoodResource
    {
        IEnumerable<FoodResourceDTO> GetFoodResources();
        FoodResource GetFoodResource(int id);
        FoodResource PutFoodResource(FoodResource foodResource);
        FoodResourceDTO InsertFoodResource(AddFoodResourceDTO insertItem);
        FoodResource DeleteFoodResource(int id);
    }
}


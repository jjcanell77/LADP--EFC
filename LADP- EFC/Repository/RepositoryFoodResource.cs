using LADP__EFC.Data;
using LADP__EFC.Models;
using LADP__EFC.Repository.Interfaces;

namespace LADP__EFC.Repository
{
    public class RepositoryFoodResource : IRepositoryFoodResource
    {
        private readonly DataContext _context;

        public RepositoryFoodResource(DataContext context)
        {
            _context = context;
        }
        public FoodResource DeleteFoodResource(int id)
        {
            throw new NotImplementedException();
        }

        public FoodResource GetFoodResource(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FoodResource> GetFoodResources()
        {
            throw new NotImplementedException();
        }

        public FoodResource PostFoodResource(FoodResource foodResource)
        {
            throw new NotImplementedException();
        }

        public FoodResource PutFoodResource(int id, FoodResource foodResource)
        {
            throw new NotImplementedException();
        }
    }
}

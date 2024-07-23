using LADP__EFC.Data;
using LADP__EFC.Models;
using LADP__EFC.Repository.Interfaces;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

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
            /*  Another way to write more closely resembling SQL queries
                var todoItems = from item in _context.TodoItems
                                select ItemToDTO(item);
                return todoItems.ToList();
            */
            return _context.FoodResources
                            .Include(r => r.ResourceTags).ThenInclude(rt => rt.Tag)
                            .Include(b => b.BusinessHours).ThenInclude(bh => bh.Day)
                            .ToList();
        }

        public FoodResource PostFoodResource(FoodResource foodResource)
        {
            // Ensure Tags are properly handled
            foreach (var resourceTag in foodResource.ResourceTags)
            {
                var tag = _context.Tags.FindAsync(resourceTag.TagId);
                if (tag == null)
                {
                    throw new Exception($"Tag with Id {resourceTag.TagId} does not exist.");
                }
                resourceTag.Tag = tag;
            }

            // Ensure Days are properly handled for BusinessHours
            foreach (var businessHour in foodResource.BusinessHours)
            {
                var day = _context.Days.FindAsync(businessHour.DayId);
                if (day == null)
                {
                    throw new Exception($"Day with Id {businessHour.DayId} does not exist.");
                }
                businessHour.Day = day;
            }

            return _context.FoodResources.Add(foodResource);
        }

        public FoodResource PutFoodResource(int id, FoodResource foodResource)
        {
            throw new NotImplementedException();
        }
    }
}

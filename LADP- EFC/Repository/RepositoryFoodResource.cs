using Azure;
using LADP__EFC.Data;
using LADP__EFC.DTO;
using LADP__EFC.Models;
using LADP__EFC.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            var item = _context.FoodResources
               .Include(fr => fr.Tags).ThenInclude(rt => rt.Name)
               .Include(fr => fr.BusinessHours).ThenInclude(bh => bh.Day)
               .FirstOrDefault(fr => fr.Id == id);
            if (item == null)
            {
                throw new Exception($"FoodResource with Id {id} does not exist.");
            }
            return item;
            //throw new NotImplementedException();
        }

        public IEnumerable<FoodResourceDTO> GetFoodResources()
        {

            // Using method-based query syntax.
            var foodResource = _context.FoodResources
                            .Include(r => r.Tags)
                            .Include(b => b.BusinessHours).ThenInclude(bh => bh.Day)
                            .Select(x => MapFoodResource(x))
                            .ToList();

            return foodResource;
        }


        // why not use async
        // public async Task<in>t PostFoodResource(InsertUpdateItem foodResource)
        public FoodResourceDTO InsertFoodResource(AddFoodResourceDTO insertItem)
        {
            // Create new instance of your entity without setting Id
            var newFoodResource = new FoodResource
            {
                Name = insertItem.Name,
                Area = insertItem.Area,
                StreetAddress = insertItem.StreetAddress,
                City = insertItem.City,
                State = insertItem.State,
                Zipcode = insertItem.Zipcode,
                Country = insertItem.Country,
                Latitude = insertItem.Latitude,
                Longitude = insertItem.Longitude,
                Phone = insertItem.Phone,
                Website = insertItem.Website,
                Description = insertItem.Description,
            };

            // Add newFoodResource to context and save changes to get its ID
            _context.FoodResources.Add(newFoodResource);
            _context.SaveChanges();

            foreach (var tagDTO in insertItem.Tags)
            {
                // Check if tag exists
                var tag = _context.Tags.FirstOrDefault(t => t.Name == tagDTO.Name);
                if (tag == null) // If it does not exist, create it
                {
                    tag = new Tag { Name = tagDTO.Name };
                    _context.Tags.Add(tag);
                    _context.SaveChanges(); // Save to get the tag ID
                }
                // Handle the join table ResourceTags
                var resourceTag = new ResourceTags { TagId = tag.Id, FoodResourceId = newFoodResource.Id };
                _context.ResourceTags.Add(resourceTag);
                newFoodResource.Tags.Add(tag);
            }

            // Ensure Days are properly handled for BusinessHours
            var allDays = _context.Days.ToList();
            foreach (var day in allDays)
            {
                var bhDTO = insertItem.BusinessHours.FirstOrDefault(b => b.Day.Name == day.Name);
                // Create BusinessHours with null OpenTime and CloseTime for missing days
                newFoodResource.BusinessHours.Add(new BusinessHours
                {
                    Day = day,
                    OpenTime = bhDTO?.OpenTime,
                    CloseTime = bhDTO?.CloseTime,
                    FoodResource = newFoodResource
                });
            }

            _context.SaveChanges();
            return MapFoodResource(newFoodResource);
        }


        public FoodResource PutFoodResource(FoodResource foodResource)
        {
            throw new NotImplementedException();
        }

        private static FoodResourceDTO MapFoodResource(FoodResource item)
        {
            var mappedItem =  new FoodResourceDTO
            {
                Id = item.Id,
                Name = item.Name,
                Area = item.Area,
                StreetAddress = item.StreetAddress,
                City = item.City,
                State = item.State,
                Zipcode = item.Zipcode,
                Country = item.Country,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Phone = item.Phone,
                Website = item.Website,
                Description = item.Description            };
            if(item.Tags.Count > 0)
            {
                foreach(var tag in item.Tags)
                {
                    mappedItem.Tags.Add(new TagDTO { Name = tag.Name });
                }
            }
            foreach (var bh in item.BusinessHours)
            {
                mappedItem.BusinessHours.Add(new BusinessHoursDTO
                {
                    Day = new DayDTO { Name = bh.Day.Name},
                    OpenTime = bh?.OpenTime,
                    CloseTime = bh?.CloseTime,
                });
            }
            return mappedItem;
        }
    }
}

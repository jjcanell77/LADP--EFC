using Microsoft.AspNetCore.Mvc;
using LADP__EFC.Models;
using LADP__EFC.Repository.Interfaces;
using LADP__EFC.DTO;
using NuGet.Protocol.Core.Types;

namespace LADP__EFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodResourcesController : ControllerBase
    {
        private readonly IRepositoryFoodResource _repository;
        public FoodResourcesController(IRepositoryFoodResource repository)
        {
            _repository = repository;
        }

        // GET: api/FoodResources
        [HttpGet]
        public ActionResult<IEnumerable<FoodResourceDTO>> GetFoodResources()
        {
            try
            { 
                var list = _repository.GetFoodResources();
                if (list == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(list);
                }
            }
            catch (Exception ex) 
            {
                int iCode = 500;
                return StatusCode(iCode, ex.ToString());
            }
        }

        // GET: api/FoodResources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodResource>> GetFoodResource(int id)
        {
            var foodResource = _repository.GetFoodResource(id);

            if (foodResource == null)
            {
                return NotFound();
            }

            return foodResource;
        }

        // PUT: api/FoodResources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult<FoodResource> PutFoodResource(int id, FoodResource foodResource)
        {
            if (id != foodResource.Id)
            {
                return BadRequest();
            }

            var item = _repository.PutFoodResource( foodResource);
            if (item != null)
            {
                return NoContent();
            }
            return NotFound();
        }

        // POST: api/FoodResources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // verify the benefits of using Async
        /* public async Task<IActionResult> InsertFoodResource(FoodResourceAddDTO insertItem)*
           {
                var id = await _repository.InsertFoodResource(insertItem);
                return Created(string.Empty, new { id });
        }
         */
        [HttpPost]
        //public ActionResult<FoodResourceAddDTO> PostFoodResource(FoodResourceAddDTO foodResource)
        public ActionResult<FoodResourceDTO> PostFoodResource(AddFoodResourceDTO foodResource)
        {
            ObjectResult result;
            try
            {
                var item = _repository.InsertFoodResource(foodResource);
                result = CreatedAtAction("GetFoodResource", new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                int iCode = 500;
                result = StatusCode(iCode, ex.ToString());
            }
            return result;
        }

        // DELETE: api/FoodResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodResource(int id)
        {
            var item = _repository.DeleteFoodResource(id);
            if (item == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

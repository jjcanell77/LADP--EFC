using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LADP__EFC.Data;
using LADP__EFC.Models;
using LADP__EFC.Repository.Interfaces;

namespace LADP__EFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodResourcesController : ControllerBase
    {
        private readonly IRepositoryFoodResource _repositroy;
        public FoodResourcesController(IRepositoryFoodResource repository)
        {
            _repositroy = repository;
        }

        // GET: api/FoodResources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodResource>>> GetFoodResources()
        {
            var foodResource = _repositroy.GetFoodResources();
            return Ok(foodResource);
        }

        // GET: api/FoodResources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodResource>> GetFoodResource(int id)
        {
            var foodResource =  _repositroy.GetFoodResource(id);

            if (foodResource == null)
            {
                return NotFound();
            }

            return foodResource;
        }

        // PUT: api/FoodResources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodResource(int id, FoodResource foodResource)
        {
            if (id != foodResource.Id)
            {
                return BadRequest();
            }

            var item = _repositroy.PutFoodResource(id, foodResource);
            if (item != null)
            {
                return NoContent();
            }
            return NotFound();
        }

        // POST: api/FoodResources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodResource>> PostFoodResource(FoodResource foodResource)
        {
            var item = _repositroy.PostFoodResource(foodResource);
            return CreatedAtAction("GetFoodResource", new { id = foodResource.Id }, foodResource);
        }

        // DELETE: api/FoodResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodResource(int id)
        {
            var item = _repositroy.DeleteFoodResource(id);
            if (item == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

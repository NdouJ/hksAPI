using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    public class DogController : Controller
    {
        ICrudGeneric<Dog> _dogRepository;
        public DogController(ICrudGeneric<Dog> dogRepository)
        {
            _dogRepository = dogRepository;
        }

        [Authorize]
        [HttpGet("get-all-dogs")]
        public IEnumerable<Dog> Get()
        {
            return _dogRepository.GetAll();
        }

        [HttpGet("get-by-name")]
        public ActionResult<Dog> Get(string name )
        {
            return _dogRepository.GetByName(name); 
        }

        [HttpGet("{id}")]
        public ActionResult<Dog> Get(int id)
        {
            var review = _dogRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            return review;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Dog value)
        {
            _dogRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = value.IdDog }, value);

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Dog value)
        {
            var existingDog = _dogRepository.GetById(id);
            if (existingDog == null)
            {
                return NotFound();
            }
           
            _dogRepository.Update(value);
            return NoContent();
        }

        // DELETE api/<UserReviewController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var review = _dogRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            _dogRepository.Delete(id);
            return NoContent();
        }
    }
}

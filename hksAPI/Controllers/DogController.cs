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
        public IActionResult Get()
        {
            try
            {
                return Ok( _dogRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        [HttpGet("get-by-name")]
        public IActionResult Get(string name )
        {
            try
            {
                var response = _dogRepository.GetByName(name);
                if (response==null)
                {
                    return NotFound();
                }
                return  Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            try
            {
                var review = _dogRepository.GetById(id);
                if (review == null)
                {
                    return NotFound();
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }  

        }

        [HttpPost]
        public IActionResult Post([FromBody] Dog value)
        {

            try
            {
                _dogRepository.Insert(value);
                return CreatedAtAction(nameof(Get), new { id = value.IdDog }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
     

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Dog value)
        {       
            try
            {
                var existingDog = _dogRepository.GetById(id);
                if (existingDog == null)
                {
                    return NotFound();
                }

                _dogRepository.Update(value);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // DELETE api/<UserReviewController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
       
            try
            {
                var review = _dogRepository.GetById(id);
                if (review == null)
                {
                    return NotFound();
                }
                _dogRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}

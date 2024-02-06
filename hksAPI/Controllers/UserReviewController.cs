using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {

        ICrudGeneric<UserReview> _userReviewRepository;
        public UserReviewController(ICrudGeneric<UserReview> userReviewRepository)
        {
            _userReviewRepository = userReviewRepository;
        }

        [HttpGet("get-all-review")]
        public IEnumerable<UserReview> Get()
        {
            return _userReviewRepository.GetAll();
        }

 

        // GET api/<UserReviewController>/5
        [HttpGet("{id}")]
        public ActionResult<UserReview> Get(int id)
        {
            var review = _userReviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            return review;
        }

        // POST api/<UserReviewController>
        [HttpPost]
        public IActionResult Post([FromBody] UserReview value)
        {
            _userReviewRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = value.IdUserReview }, value);

        }

        // PUT api/<UserReviewController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserReview value)
        {
            var existingReview = _userReviewRepository.GetById(id);
            if (existingReview == null)
            {
                return NotFound();
            }
            value.IdUserReview = id;
            _userReviewRepository.Update(value);
            return NoContent();
        }

        // DELETE api/<UserReviewController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var review = _userReviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            _userReviewRepository.Delete(id);
            return NoContent();
        }
    }
}

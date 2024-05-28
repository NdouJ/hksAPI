using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private readonly ICrudGeneric<UserReview> _userReviewRepository;

        public UserReviewController(ICrudGeneric<UserReview> userReviewRepository)
        {
            _userReviewRepository = userReviewRepository;
        }

        [HttpGet("get-all-review")]
        public IActionResult GetAllReviews()
        {
            try
            {
                var reviews = _userReviewRepository.GetAll();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            try
            {
                var review = _userReviewRepository.GetById(id);
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
        public IActionResult CreateReview([FromBody] UserReview value)
        {
            try
            {
                _userReviewRepository.Insert(value);
                return CreatedAtAction(nameof(GetReviewById), new { id = value.IdUserReview }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] UserReview value)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                var review = _userReviewRepository.GetById(id);
                if (review == null)
                {
                    return NotFound();
                }
                _userReviewRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

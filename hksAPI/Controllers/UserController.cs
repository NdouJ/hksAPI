using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICrudGeneric<User> _userRepository;

        public UserController(ICrudGeneric<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult SaveUser([FromBody] User user)
        {
            try
            {
                _userRepository.Insert(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("update-user")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            try
            {
                _userRepository.Update(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

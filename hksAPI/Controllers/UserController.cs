using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
       
        [HttpPost("checkUser")]
        public IActionResult CheckUser([FromBody] User user)
        {
            try
            {
                string check = _userRepository.CheckEntity(user);

                if (check == "TRUE")
                {
                    return Ok();

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getOathUserByUsername")]
        public IActionResult GetOathUserbyUsername(string name)
        {
            try
            {
                User user = _userRepository.GetByName(name);

                if (user!=null)
                {
                    return Ok(user);

                }
                else
                {
                    return NotFound(); 
                }
        }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
}
    }
}

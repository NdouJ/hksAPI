using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ICrudGeneric<User> _userRepository;
        public UserController(ICrudGeneric<User> userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpPost]
        public IActionResult SaveUser([FromBody] User user) {

            try
            {
                _userRepository.Insert(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message); 
            }


            return Ok(); 
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
         
            _userRepository.Delete(id);
            return NoContent();
        }



        [HttpPost("update-user")]
        public IActionResult UpdateUser([FromBody] User user)
        {

            try
            {
                _userRepository.Update(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


            return Ok();
        }
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}

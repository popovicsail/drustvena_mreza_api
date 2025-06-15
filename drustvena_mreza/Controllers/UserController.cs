using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using drustvena_mreza.Models;
using drustvena_mreza.Utilities;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [ApiController]
    [Route("api/users")]
    
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UserController(IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
        }


        [HttpGet]
        public ActionResult GetPaged ([FromQuery] int page, [FromQuery] int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be higher than 0");
            }

            try
            {
                List<User> pagedUser = userRepository.GetPaged(page, pageSize);
                int totalCount = userRepository.CountAll();

                Object result = new
                {
                    Data = pagedUser,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                return Problem("ERROR: An error occured.");
            }
        }


        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            try
            {
                User user = userRepository.GetById(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(user);
            }
            catch (Exception exception)
            {
                return Problem("An error occurred while fetching the user.");
            }
        }


        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.DateOfBirth.ToString()))
            {
                return BadRequest();
            }

            try
            {
                User newUser = userRepository.Create(user);
                return Ok(newUser);
            }
            catch (Exception exception)
            {
                return Problem("An error occurred while creating the user.");
            }
        }


        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.DateOfBirth.ToString()))
            {
                return BadRequest();
            }

            try
            {
                user.Id = id;
                User updatedUser = userRepository.Update(user);
                if (updatedUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(updatedUser);
            }
            catch (Exception exception)
            {
                return Problem("An error occurred while updating the user.");
            }
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool isDeleted = userRepository.Delete(id);
                if (!isDeleted)
                {
                    return NotFound($"User with ID {id} not found.");               
                }
                return NoContent();

            }
            catch (Exception exception)
            {
                return Problem("An error occurred while deleting the user.");
            }
        }
    }
}

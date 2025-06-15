using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using drustvena_mreza.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupRepository groupRepository;

        public GroupController(IConfiguration configuration)
        {
            groupRepository = new GroupRepository(configuration);
        }

        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page, [FromQuery] int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("ERROR: Page and PageSize must be higher than 0");
            }
            try
            {
                List<Group> pagedGroup = groupRepository.GetPaged(page, pageSize);
                int totalCount = groupRepository.CountAll();

                Object result = new
                {
                    Data = pagedGroup,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                return Problem("ERROR: ");

            }      
        }


        [HttpGet("{id}")]
        public ActionResult<Group> GetById(int id)
        {
            try
            {
                Group group = groupRepository.GetById(id);
                if (group == null)
                {
                    return NotFound($"Group with ID {id} not found.");
                }
                return Ok(group);
            }
            catch (Exception exception)
            {
                return Problem("ERROR: ");
            }
        }

        [HttpPost]
        public ActionResult<Group> Create([FromBody] Group group)
        {
            if (string.IsNullOrWhiteSpace(group.Name) || string.IsNullOrWhiteSpace(group.DateOfCreation.ToString()))
            {
                return BadRequest();
            }

            try
            {
                Group createdGroup = groupRepository.Create(group);
                return Ok(createdGroup);
            }
            catch (Exception exception)
            {
                return Problem("ERROR: ");
            }
        }


        [HttpPut("{id}")]
        public ActionResult<Group> Update(int id, [FromBody] Group group)
        {
            if (string.IsNullOrWhiteSpace(group.Name) || group.DateOfCreation == DateTime.MinValue)
            {
                return BadRequest("Invalid group data.");
            }

            try
            {
                group.Id = id;
                Group updatedGroup = groupRepository.Update(group);
                if (updatedGroup == null)
                {
                    return NotFound($"Group with ID {id} not found.");
                }
                return Ok(updatedGroup);
            }
            catch (Exception exception)
            {
                return Problem("ERROR: ");
            }
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool isDeleted = groupRepository.Delete(id);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound($"Group with ID {id} not found.");
            }
            catch (Exception exception)
            {
                return Problem("ERROR: ");
            }
        }
    }
}

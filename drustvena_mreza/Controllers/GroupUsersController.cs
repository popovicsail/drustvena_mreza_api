using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupUsersController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly GroupRepository groupRepository;
        private readonly GroupUsersRepository groupUsersRepository;

        public GroupUsersController(IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
            groupRepository = new GroupRepository(configuration);
            groupUsersRepository = new GroupUsersRepository(configuration);
        }

        [HttpPut("{userId}")]
        public ActionResult<Group> Add(int groupId, int userId)
        {
            Group newGroup = groupRepository.GetById(groupId);
            if (newGroup == null)
            {
                return NotFound("Group not found");
            }

            User newUser = userRepository.GetById(userId);
            if (newUser == null)
            {
                return NotFound("User not found");
            }

            foreach (User groupUser in newGroup.GroupUsers)
            {
                if (groupUser.Id == userId)
                {
                    return Conflict();
                }
            }
            newGroup.GroupUsers.Add(newUser);

            try
            {
                groupUsersRepository.Add(groupId, userId);
                return Ok(newGroup);
            }
            catch (Exception exception)
            {
                return Problem("An error occurred while adding user to group.");
            }
        }


        [HttpDelete("{userId}")]
        public ActionResult<Group> Remove(int groupId, int userId)
        {
            Group group = groupRepository.GetById(groupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }

            User user = userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            group.GroupUsers.Remove(user);
            groupUsersRepository.Remove(groupId, userId);

            return Ok(group);
        }


        [HttpGet("management")]
        public ActionResult GetByIdManagement(int groupId)
        {
            try
            {
                Group group = groupRepository.GetById(groupId);
                if (group == null)
                {
                    return NotFound($"Group with ID {groupId} not found.");
                }
                List<User> groupUsers = group.GroupUsers;

                List<User> allUser = userRepository.GetAll();

                List<User> groupNonUsers = allUser.Where(u => !groupUsers.Any(m => m.Id == u.Id)).ToList();

                Object result = new
                {
                    resultGroupUsers= groupUsers,
                    resultGroupNonUsers = groupNonUsers
                };

                return Ok(result);
            }
            catch (Exception exception)
            {
                return Problem("ERROR: ");
            }
        }
    }
}

using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/users/{userId}/posts")]
    [ApiController]
    public class UserPostsController : ControllerBase
    {
        private readonly PostRepository postRepository;
        private readonly UserRepository userRepository;

        public UserPostsController(IConfiguration configuration)
        {
            postRepository = new PostRepository(configuration);
            userRepository = new UserRepository(configuration);
        }

        [HttpPost]
        public ActionResult<Post> CreatePost(int userId, [FromBody] Post post)
        {
            User user = userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            post.Author = user;
            try
            {
                Post createdPost = postRepository.Create(post);
                return createdPost;
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while creating the post.");
            }
        }
    }
}

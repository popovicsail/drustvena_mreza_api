using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly PostRepository postRepository;

        public PostsController(IConfiguration configuration)
        {
            postRepository = new PostRepository(configuration);
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAll()
        {
            List<Post> posts = postRepository.GetAll();
            return Ok(posts);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool isDeleted = postRepository.Delete(id);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound($"Post with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while deleting the post.");
            }
        }
    }
}

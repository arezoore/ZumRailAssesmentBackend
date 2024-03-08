using Microsoft.AspNetCore.Mvc;
using assessment.Model;
using assessment.Shared.Repository;
using assessment.Shared.Interface;
using assessment.Model.Request;

namespace assessment.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController :  ControllerBase
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string tags,string? sortBy,string? direction)
        {
            string[] validDirections = new string[]{"asc","desc"};
            string[] validSortParameters = new string[]{"id","reads","likes","popularity"};
            if(!validSortParameters.Contains(sortBy?.ToLower())&&sortBy!=null){
                return BadRequest("Invalid Input");
            }
             if(!validDirections.Contains(direction?.ToLower()) && direction!=null){
                return BadRequest("Invalid Input");
             }
            var model = new SearchByTagViewModel(){
                tags = tags.Split(',').ToList(),
                sortBy = sortBy,
                direction = direction
            };
            var postsLists = await _blogRepository.GetPosts(model);
            if(!String.IsNullOrEmpty(model.sortBy) || !String.IsNullOrEmpty(model.direction))
            {
                postsLists.posts = _blogRepository.SortBlogList(postsLists.posts,out string message,model.sortBy,model.direction);
            }
            return Ok(postsLists);
        }

    }
}
using assessment.Model;
using assessment.Model.Request;
using assessment.Model.Response;

namespace assessment.Shared.Interface
{
    public interface IBlogRepository
    {
        public  Task<ResponseViewModel> GetPosts(SearchByTagViewModel tag);
        public List<PostResponseViewModel> SortBlogList(List<PostResponseViewModel> model,out string message,string sortBy="id",string direction="asc");

    }
}
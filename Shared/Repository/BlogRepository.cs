using assessment.Model.Request;
using assessment.Model.Response;
using assessment.Shared.Interface;
using Microsoft.AspNetCore.Mvc;
using assessment.Model;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft;
using Newtonsoft.Json;

namespace assessment.Shared.Repository
{
    public class BlogRepository : IBlogRepository
    {
        
        public async Task<ResponseViewModel> GetPosts(SearchByTagViewModel search)
        {
            var response = new ResponseViewModel();
            response.posts = new List<PostResponseViewModel>();

            foreach(var item in search.tags)
            {
                string Url = $"https://api.hatchways.io/assessment/blog/posts?tag={item}";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Url);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpsResponse = client.GetAsync(Url).Result;  
                if (httpsResponse.IsSuccessStatusCode)
                {
                    ResponseViewModel result = JsonConvert.DeserializeObject<ResponseViewModel>(await httpsResponse.Content.ReadAsStringAsync());
                    response.posts.AddRange(result.posts);
                }
                else
                {
                    Console.WriteLine("Error while fetching the data");
                }
            }
            response.posts = response.posts.DistinctBy(x=>x.id).OrderBy(x=>x.id).ToList();
            return response;
        }
        public List<PostResponseViewModel> SortBlogList(List<PostResponseViewModel> model,out string message,string sortBy="id",string direction="asc")
        {
            string[] validDirections = new string[]{"asc","desc"};
            string[] validSortParameters = new string[]{"id","reads","likes","popularity"};
            if(direction==null)
                direction = "asc";
            if(sortBy==null)
                sortBy = "id";
            if(!validSortParameters.Contains(sortBy.ToLower()))
            {
                //we can throw a handled exceptions as well
                message = "sortBy parameter is invalid";
                return model;
            }
            if(!validDirections.Contains(direction.ToLower()))
            {
                //we can throw a handled exceptions as well
                message = "direction parameter is invalid";
                return model;
            }
            switch(sortBy.ToLower()){
                case "id":
                {
                    if(direction.ToLower().Equals("asc"))
                    {
                        model = model.OrderBy(x=>x.id).ToList();
                    }
                    else{
                        model = model.OrderByDescending(x=>x.id).ToList();
                    }
                    break;
                }
                case "reads":
                {
                    if(direction.ToLower().Equals("asc"))
                    {
                        model = model.OrderBy(x=>x.reads).ToList();
                    }
                    else{
                        model = model.OrderByDescending(x=>x.reads).ToList();
                    }
                    break;
                }
                case "likes":
                {
                    if(direction.ToLower().Equals("asc"))
                    {
                        model = model.OrderBy(x=>x.likes).ToList();
                    }
                    else{
                        model = model.OrderByDescending(x=>x.likes).ToList();
                    }
                    break;
                }
                case "popularity":
                {
                    if(direction.ToLower().Equals("asc"))
                    {
                        model = model.OrderBy(x=>x.popularity).ToList();
                    }
                    else{
                        model = model.OrderByDescending(x=>x.popularity).ToList();
                    }
                    break;
                }
                default:
                {
                    if(direction.ToLower().Equals("asc"))
                    {
                        model = model.OrderBy(x=>x.id).ToList();
                    }
                    else{
                        model = model.OrderByDescending(x=>x.id).ToList();
                    }
                    break;
                }
            }
            message = string.Empty;
            return model;
        }
    }
}
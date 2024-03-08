using System.ComponentModel.DataAnnotations;
using assessment.Model;

namespace assessment.Model.Request
{
    public class SearchByTagViewModel : SearchViewModel
    {
        [Required(ErrorMessage="Tags are mandatory.")]
        public List<string> tags { get; set; }
    }
}
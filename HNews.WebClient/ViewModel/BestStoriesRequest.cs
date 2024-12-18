using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HNews.WebClient.ViewModel
{
    public class BestStoriesRequest
    {
        [Required]
        [Display(Name = "Number of best stories")]
        public int N_param { get; set; }

        [Required]
        [Display(Name = "Comments replies counts like comments?")]
        public bool setRecursive { get; set; }
    }
}

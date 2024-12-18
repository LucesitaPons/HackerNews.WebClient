using HNews.WebClient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNews.WebClient.Controllers
{
    public class HackerNewsApiController : Controller
    {
        private readonly HackerNewsApiService _hackerNewsApiService;
        public HackerNewsApiController(HackerNewsApiService hackerNewsApiService)
        {
            _hackerNewsApiService = hackerNewsApiService;
        }

        // GET: HackerNewsApiController
        public async Task<ActionResult> Index()
        {
            var topStories = await _hackerNewsApiService.GetStoriesAsync();
            return View(topStories);
        }

        // GET: HackerNewsApiController/BestStoriesDetails

        public async Task<ActionResult> BestStoriesDetails(int n_param, bool setRecursive)
        {
            var bestStories = await _hackerNewsApiService.GetBestStoriesAsync(n_param, setRecursive);

            return View(bestStories);
        }
    }
}

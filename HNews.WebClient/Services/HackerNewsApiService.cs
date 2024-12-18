using HNews.WebClient.Models;
using Newtonsoft.Json;

namespace HNews.WebClient.Services
{
    public class HackerNewsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(10);

        public HackerNewsApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = httpClient.BaseAddress.ToString();
        }

        /// <summary>
        /// Gets the Stories (testing method)
        /// </summary>
        /// <returns></returns>
        public async Task<List<int?>?> GetStoriesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/topstories.json");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<int?>?>(responseBody);
        }

        /// <summary>
        /// Gets the Item Details an returns the JSon string
        /// </summary>
        /// <param name="itemId">Item ID</param>
        /// <returns>JSon string with all the Item detail</returns>
        public async Task<string?> GetItemJSonAsync (int itemId)
        {
            var responseDetails = await _httpClient.GetAsync($"{_baseUrl}/item/{itemId}.json");
            responseDetails.EnsureSuccessStatusCode();

            var responseDetailsBody = await responseDetails.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseDetailsBody))
            {
                return null;
            }
            return responseDetailsBody;
        }

        /// <summary>
        /// Gets the Best Stories with all the data and comments information
        /// </summary>
        /// <param name="n_param">Number of Results desired by user</param>
        /// <param name="setRecursive">True to count also comments replies</param>
        /// <returns></returns>
        public async Task<List<BestStories>?> GetBestStoriesAsync(int n_param, bool setRecursive = false)
        {
            //Getting all the beststories IDs
            var responseIds = await _httpClient.GetAsync($"{_baseUrl}/beststories.json");
            responseIds.EnsureSuccessStatusCode();

            var responseIdsBody = await responseIds.Content.ReadAsStringAsync();

            if(responseIdsBody == null)
            {
                return new List<BestStories>();
            }

            List<int>? bestStoriesIds = JsonConvert.DeserializeObject<List<int>?>(responseIdsBody);

            if(bestStoriesIds == null)
            {
                return new List<BestStories>();
            }

            List<BestStories> bestStories = await GetBestStoriesDetailsAsync(bestStoriesIds);

            //Getting only the number of items required by the user
            bestStories = bestStories.Take(n_param).ToList();

            //Getting all the comments by story

            foreach (BestStories bs in bestStories.Where(k => k.Kids != null && k.Kids.Count > 0))
            {
                //Getting all comments (and replies) of a single BestStorie
                if (bs.Kids != null && bs.Kids.Count > 0)
                {
                    bs.Comments = await GetCommentsTypeKidsAsync(bs.Kids, setRecursive);
                    bs.CommentsCount = bs.Comments.Count();
                }
            }

            return bestStories.ToList();

        }

        /// <summary>
        /// Gets the details of the Best Stories
        /// </summary>
        /// <param name="bestStoriesIds">Best Stories Ids list</param>
        /// <returns>Returns a List of BestStories with details</returns>
        public async Task<List<BestStories>> GetBestStoriesDetailsAsync(List<int> bestStoriesIds)
        {
            var tasks = bestStoriesIds.Select(id => GetBestStoriesDetailsAsync(id)).ToArray();
            var stories = await Task.WhenAll(tasks);

            return stories.ToList().OrderByDescending(x => x.Score).ToList(); 
        }

        /// <summary>
        /// Gets the details of a single BestStories item
        /// </summary>
        /// <param name="itemId">Item ID </param>
        /// <returns>Single best story</returns>
        /// <exception cref="Exception"></exception>
        private async Task<BestStories> GetBestStoriesDetailsAsync(int itemId)
        {
            await _semaphore.WaitAsync(); 
            try
            {
                var response = await GetItemJSonAsync(itemId);

                if (string.IsNullOrEmpty(response))
                {
                    return new BestStories();
                }

                var bs = JsonConvert.DeserializeObject<BestStories>(response);

                if (bs == null)
                {
                    return new BestStories();
                }
                return bs;
            }
            catch 
            {
                throw new Exception("Semaphore error.");
            }
            finally
            {
                _semaphore.Release();
            }
            
        }

        /// <summary>
        /// Gets a coment details
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Returns a single comment with details</returns>
        /// <exception cref="Exception"></exception>
        private async Task<Comment> GetCommentDetailsAsync(int itemId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var response = await GetItemJSonAsync(itemId);

                if (string.IsNullOrEmpty(response))
                {
                    return new Comment();
                }

                var comment = JsonConvert.DeserializeObject<Comment>(response);

                if (comment == null)
                {
                    return new Comment();
                }
                return comment;
            }
            catch
            {
                throw new Exception("Semaphore error.");
            }
            finally
            {
                _semaphore.Release();
            }

        }

        /// <summary>
        /// Searching all the Kids type Comment
        /// </summary>
        /// <param name="kids">Kids item list</param>
        /// <param name="recursive">True to search in the comments replies for more comments</param>
        /// <returns></returns>
        public async Task<List<Comment>?> GetCommentsTypeKidsAsync(List<int> kids, bool recursive = false)
        {

            var tasks = kids.Select(id => GetCommentDetailsAsync(id)).ToArray();
            var commentsAux = await Task.WhenAll(tasks);

            var commentList = commentsAux.ToList();

            if (!recursive)
                return commentList;

            //If reply to the comments exists, run the process again (only if recursive).
            var commentReplies = commentsAux.Where(subComment => subComment.Kids != null && subComment.Kids.Count > 0);
            if (commentReplies!=null && commentReplies.Count() > 0 && recursive)
            {
                foreach(var reply in commentReplies)
                {
                    if (reply != null && reply.Kids!=null && reply.Kids.Count>0)
                    {
                        var replies = await GetCommentsTypeKidsAsync(reply.Kids, recursive);

                        if (replies != null)
                            commentList.AddRange(replies);
                    }
                }
            }
            return commentList;

        }


    }
}

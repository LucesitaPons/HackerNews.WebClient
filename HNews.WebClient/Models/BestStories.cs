namespace HNews.WebClient.Models
{
    public class BestStories
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? By { get; set; }
        public long? Time { get; set; }
        public int? Score { get; set; }
        public string Type { get; set; }
        public string? Descendants { get; set; }
        public List<int>? Kids { get; set; }
        public List<Comment> Comments { get; set; }
        public int CommentsCount { get; set; }
    }
}

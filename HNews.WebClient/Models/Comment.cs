namespace HNews.WebClient.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string By { get; set; }
        public int Parent { get; set; }
        public string Text { get; set; }
        public long Time { get; set; }
        public string Type { get; set; }
        public List<int>? Kids { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}

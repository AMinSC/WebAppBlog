using System.ComponentModel.DataAnnotations;

namespace WebAppBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? content { get; set; }
        public string? UrlSlug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

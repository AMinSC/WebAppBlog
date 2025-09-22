using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public required string UrlSlug { get; set; }
        public required int CategoryId { get; set; }
        public required Categories? Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

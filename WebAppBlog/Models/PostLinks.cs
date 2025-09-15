using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppBlog.Models
{
    public class PostLinks
    {
        public int Id { get; set; }
        [ForeignKey("post")]
        public int SourcePostId { get; set; }
        [ForeignKey("post")]
        public int TargetPostId { get; set; }
    }
}

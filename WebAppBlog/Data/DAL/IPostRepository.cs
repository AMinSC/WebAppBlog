using WebAppBlog.Models;

namespace WebAppBlog.Data.DAL
{
    public interface IPostRepository : IDisposable
    {
        IEnumerable<Post> GetAllPosts();
        Post? GetPostById(int postId);
        void InsertPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(Post post);
        void Save();
    }
}

using WebAppBlog.Models;

namespace WebAppBlog.Data.DAL
{
    public class PostRepository : IPostRepository, IDisposable
    {
        private readonly WebAppBlogContext _context;
        private bool disposed = false;

        public PostRepository(WebAppBlogContext context)
        {
            _context = context;
        }

        
        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Post.ToList();
        }

        public Post? GetPostById(int postId)
        {
            return _context.Post.Find(postId);
        }

        public void InsertPost(Post post)
        {
            _context.Post.Add(post);
        }
        public void UpdatePost(Post post)
        {
            _context.Post.Update(post);
        }

        public void DeletePost(Post post)
        {
            _context.Post.Remove(post);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

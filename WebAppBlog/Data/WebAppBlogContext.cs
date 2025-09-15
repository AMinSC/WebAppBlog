using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAppBlog.Models;

namespace WebAppBlog.Data
{
    public class WebAppBlogContext : DbContext
    {
        public WebAppBlogContext (DbContextOptions<WebAppBlogContext> options)
            : base(options)
        {
        }

        public DbSet<WebAppBlog.Models.Post> Post { get; set; } = default!;
        public DbSet<WebAppBlog.Models.PostLinks> PostLinks { get; set; } = default!;
    }
}

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

        public DbSet<Post> Post { get; set; } = default!;
        public DbSet<Categories> Categories { get; set; } = default!;
    }
}

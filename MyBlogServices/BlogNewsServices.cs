using MyBlog.Model;
using MyBlogIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogServices
{
    public class BlogNewsServices:BaseServices<BlogNews>,IBlogNewServices
    {
        private readonly BlogNewsServices _blogNewServices;
        public BlogNewsServices(BlogNewsServices iBlogNewServices)
        {
            base._services = iBlogNewServices;
            _blogNewServices = iBlogNewServices;
        }
    }
}

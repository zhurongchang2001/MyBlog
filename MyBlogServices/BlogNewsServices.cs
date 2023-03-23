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
        private readonly IBlogNewServices _blogNewServices;
        public BlogNewsServices(IBlogNewServices iBlogNewServices)
        {
            base._services = iBlogNewServices;
            _blogNewServices = iBlogNewServices;
        }
    }
}

using MyBlog.IRepository;
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
        private readonly IBlogNewsRepository _blogNewServices;
        public BlogNewsServices(IBlogNewsRepository iBlogNewServices)
        {
            base._services = iBlogNewServices;
            this._blogNewServices = iBlogNewServices;
        }
    }
}

using MyBlogIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Model;
using MyBlog.IRepository;

namespace MyBlogServices
{
    public class TypeInfoServices : BaseServices<MyBlog.Model.TypeInfo>, ITypeInfoServices
    {
        private readonly ITypeinfoRepository typeInfoServices1;
        public TypeInfoServices(ITypeinfoRepository typeInfoServices)
        {
            base._services = typeInfoServices;
            typeInfoServices1 = typeInfoServices;
        }
    }
}

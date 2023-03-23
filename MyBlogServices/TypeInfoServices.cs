using MyBlogIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Model;
namespace MyBlogServices
{
    public class TypeInfoServices : BaseServices<MyBlog.Model.TypeInfo>, ITypeInfoServices
    {
        private readonly TypeInfoServices typeInfoServices1;
        public TypeInfoServices(TypeInfoServices typeInfoServices)
        {
            base._services = typeInfoServices;
            typeInfoServices1 = typeInfoServices;
        }
    }
}

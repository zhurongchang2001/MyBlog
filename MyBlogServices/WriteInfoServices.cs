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
    public class WriteInfoServices:BaseServices<WriteInfo>,IWriteInfoServices
    {
        private readonly IWriteInfoRepository writeInfoServices1;
        public WriteInfoServices(IWriteInfoRepository writeInfoServices) { 
        base._services = writeInfoServices;
        this.writeInfoServices1 = writeInfoServices;
        }
    }
}

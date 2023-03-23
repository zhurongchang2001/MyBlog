using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Model
{
    public class BaseId
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity=true)] //设置主键 和 自增
        public int Id { get; set; }
    }
}

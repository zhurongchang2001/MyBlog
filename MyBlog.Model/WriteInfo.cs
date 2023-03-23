using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace MyBlog.Model
{
    public class WriteInfo:BaseId
    {
        [SugarColumn(ColumnDataType ="nvarchar(20)")]
        public string Name { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar(20)")]
        public string UserName { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar(64)")]
        public string UserPwd { get; set; }
    }
}

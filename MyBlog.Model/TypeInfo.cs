using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace MyBlog.Model
{
    public class TypeInfo:BaseId
    {
        [SugarColumn(ColumnDataType = "nvarchar(20)")]
        public string Name { get; set; }
    }
}

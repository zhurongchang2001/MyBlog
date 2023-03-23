using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Model
{
    public class BlogNews:BaseId
    {
        [SugarColumn(ColumnDataType ="nvarchar(50)")]
        public string Title { get; set; }//文章标题
        [SugarColumn(ColumnDataType = "Text")]
        public string Content { get; set; }//文章内容
        public DateTime Time { get; set; }//发布时间

        
        public int LikeCount { get; set; }//点赞量
        public int BrowserCount { get; set; }//浏览量
        public int TypeId { get; set; }//文章类型id
        public int WriteId { get; set; }//作者id

        //IsIgnore=true表示 ORM 所有操作不处理这列,一般用于数据库没有这一列,ORM 非数据库列加上该特性（配置导航查询自动IsIgnore=true）
        /// <summary>
        /// 类型不映射到数据库
        /// </summary>
        [SugarColumn(IsIgnore=true)]
        public TypeInfo TypeInfo { get; set; }
        [SugarColumn(IsIgnore = true)]
        public WriteInfo WriteInfo { get; set; }

    }
}

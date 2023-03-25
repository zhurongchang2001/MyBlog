using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Model.DTO
{
    public class BlogNewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }//文章标题
        public string Content { get; set; }//文章内容
        public DateTime Time { get; set; }//发布时间
        public int LikeCount { get; set; }//点赞量
        public int BrowserCount { get; set; }//浏览量

        public string TypeInfoName { get; set; }
        public string WriteInfoName{ get; set; }


    }
}

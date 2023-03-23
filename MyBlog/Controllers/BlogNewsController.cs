using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IRepository;
using MyBlog.Model;
using MyBlog.Utility.ApiResult;
using MyBlogIServices;

namespace MyBlog.Controllers
{
    [Route("BlogNews/[controller]/[action]")]
    [ApiController]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewServices blogNewsService;
        public BlogNewsController(IBlogNewServices blogNewServices) {
        this.blogNewsService = blogNewServices;
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetBlogNews() {
            var data= await blogNewsService.QueryAsync();
            if (data == null||data.Count==0) {
                return ApiResultHelp.Error("没有更多的数据了");
            }
                return ApiResultHelp.Success(data,data.Count);
        }
        /// <summary>
        /// 根据id 查找一条数据
        /// </summary>
        /// <param name="BlogNewsId">数据id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetBlogNewsByid(int BlogNewsId)
        {
            var data = await blogNewsService.FindAsync(BlogNewsId);
            if (data == null)
            {
                return ApiResultHelp.Error("查找失败，该条数据不存在");
            }
            else
            {
                return ApiResultHelp.Success(data, "查找");
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="Tltle">标题</param>
        /// <param name="Context">内容</param>
        /// <param name="Typeid">类型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> AddBlogNews(string Tltle, string Context, int Typeid)
        {
            BlogNews blog = new BlogNews
            {
                Title = Tltle,
                Content = Context,
                TypeId = Typeid,
                Time = DateTime.Now,
                WriteId = 1,
                BrowserCount = 0,
                LikeCount = 0
            };

            bool data = await blogNewsService.CreateAsync(blog);
            if (data)
            {
                return ApiResultHelp.Success(blog, "添加");
            }
            else
            {
                return ApiResultHelp.Error("数据添加失败");
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="BlognewsId">数据id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> DeleteBlogNewById(int BlognewsId)
        {
            bool data = await blogNewsService.DeleteAsync(BlognewsId);
            if (data)
            {
                return ApiResultHelp.Success(null, "删除");
            }
            else
            {
                return ApiResultHelp.Error("该数据不存在,数据删除失败");
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="BlognewsId">数据id</param>
        /// <param name="Title">标题</param>
        /// <param name="Context">内容</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> UpdateBlogNewById(int BlognewsId, string Title, string Context) { 
            var Blognews=await blogNewsService.FindAsync(BlognewsId);
            if (Blognews == null) {
                return ApiResultHelp.Error("该数据不存在");
            }
            Blognews.Title = Title;
            Blognews.Content = Context;

            bool BlogBoll=await blogNewsService.UpdateAsync(Blognews);
            if (BlogBoll)
            {
                return ApiResultHelp.Success(BlogBoll,"修改成功");
            }
            else {
                return ApiResultHelp.Error("修改失败");
            }
        }
    }
}

using MyBlog.IRepository;
using MyBlog.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Repository
{
    public class BlogNewsRepository:BaseRepository<BlogNews>,IBlogNewsRepository
    {
        public async override  Task<List<BlogNews>> QueryAsync() { 
            //类似于两表联查，把文章表的类型赋值，取的是类型表的值
        return await base.Context.Queryable<BlogNews>()
                .Mapper(c=>c.TypeInfo,c=>c.TypeId,c=>c.TypeInfo.Id)
                .Mapper(c=>c.WriteInfo, c=>c.WriteId,c=>c.WriteInfo.Id)
                .ToListAsync();
        }

        public async override Task<List<BlogNews>> QueryAsync(Expression<Func<BlogNews, bool>> func)
        {
            return await base.Context.Queryable<BlogNews>()
                .Where(func)
                .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                .Mapper(c => c.WriteInfo, c => c.WriteId, c => c.WriteInfo.Id)
                .ToListAsync();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public async override  Task<List<BlogNews>> QueryAsync(int Page, int Size, RefAsync<int> total)
        {
            return await base.Context.Queryable<BlogNews>()
                .Mapper(c => c.WriteInfo, c => c.WriteId, c => c.WriteInfo.Id)
                .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                .ToPageListAsync(Page, Size, total);
        }
        /// <summary>
        /// 分页+条件查询
        /// </summary>
        /// <param name="func"></param>
        /// <param name="Page"></param>
        /// <param name="Size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public async override Task<List<BlogNews>> QueryAsync(Expression<Func<BlogNews, bool>> func, int Page, int Size, RefAsync<int> total)
        {
            return await base.Context.Queryable<BlogNews>()
                .Where(func)
                .Mapper(c=>c.WriteInfo,c=>c.WriteId,c=>c.WriteInfo.Id)
                .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                .ToPageListAsync(Page, Size, total);
        }

    }
}

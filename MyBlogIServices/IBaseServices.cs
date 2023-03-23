using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogIServices
{
   public  interface IBaseServices<TEntity> where TEntity : class,new()
    {
        Task<bool> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(TEntity entity);
        /// <summary>
        /// 查找一条数据
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(int id);
        Task<TEntity> FindByNameAsync(string Name);
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync();
        /// <summary>
        /// 自定义条件查询
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func);
        /// <summary>
        /// 分页查询（异步）
        /// </summary>
        /// <param name="Page">页数</param>
        /// <param name="Size">每页展示多少条数据</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(int Page, int Size, RefAsync<int> total);
        /// <summary>
        /// 自定义条件分页查询
        /// </summary>
        /// <param name="func"></param>
        /// <param name="Page"></param>
        /// <param name="Size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int Page, int Size, RefAsync<int> total);
    }
}

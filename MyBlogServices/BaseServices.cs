using MyBlog.IRepository;
using MyBlogIServices;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogServices
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        //从子类的构造函数中传入
        protected IBaseRepository<TEntity> _services;
        public async Task<bool> CreateAsync(TEntity entity)
        {
            return await _services.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _services.DeleteAsync(id);
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await _services.UpdateAsync(entity);
        }

        
        public  async Task<TEntity> FindAsync(int id)
        {
            return await _services.FindAsync(id);
        }

        public async Task<List<TEntity>> QueryAsync()
        {
            return await  _services.QueryAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _services.QueryAsync(func);
        }

        public async Task<List<TEntity>> QueryAsync(int Page, int Size, RefAsync<int> total)
        {
            return await _services.QueryAsync(Page,Size,total);
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int Page, int Size, RefAsync<int> total)
        {
            return await _services.QueryAsync(func, Page, Size, total);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _services.FindAsync(func);
        }
    }
}

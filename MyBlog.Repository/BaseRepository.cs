using MyBlog.IRepository;
using MyBlog.Model;
using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Repository
{
    public class BaseRepository<TEntity> : SimpleClient<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        public BaseRepository(ISqlSugarClient? context = null) : base(context)
        {
            base.Context = DbScoped.SugarScope;

            //创建完了后，可以注释，要不然性能有影响，这里就不注释了
            //建库：如果不存在创建数据库存在不会重复创建 
            base.Context.DbMaintenance.CreateDatabase(); // 注意 ：Oracle和个别国产库需不支持该方法，需要手动建库
            /***创建多个表***/
            //typeof(放的是实体类)
            base.Context.CodeFirst.InitTables(
                typeof(BlogNews),
                typeof(TypeInfo),
                typeof(WriteInfo)
                );//这样一个表就能成功创建了
        }

        
        //新增数据
        async Task<bool> IBaseRepository<TEntity>.CreateAsync(TEntity entity)
        {
            return await base.InsertAsync(entity);
        }
        //修改数据
        async Task<bool> IBaseRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            return await UpdateAsync(entity);
        }
        //删除数据
        async Task<bool> IBaseRepository<TEntity>.DeleteAsync(int id)
        {
            return await DeleteByIdAsync(id);
        }
        //获取所有数据
        public virtual async Task<List<TEntity>> QueryAsync()
        {
            return await GetListAsync();
        }
        //根据id查找数据
        //virtual 关键字用于修改方法、属性、索引器或事件声明，并使它们可以在派生类中被重写。
        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await GetByIdAsync(id);
        }
        //条件查询
        public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
        {
            return await GetListAsync(func);
        }
        //分页查询
        public virtual async Task<List<TEntity>> QueryAsync(int Page, int Size, RefAsync<int> total)
        {
            return await base.Context.Queryable<TEntity>().ToPageListAsync(Page, Size, total);
        }
        //分页+条件查询
        public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int Page, int Size, RefAsync<int> total)
        {
            return await base.Context.Queryable<TEntity>()
                .Where(func)
                .ToPageListAsync(Page, Size, total);
        }
        //根据名称查找数据

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func)
        {
            return await base.GetSingleAsync(func);
        }

    }
}

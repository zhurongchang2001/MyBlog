using MyBlog.IRepository;
using MyBlog.Repository;
using MyBlogIServices;
using MyBlogServices;
using SqlSugar;
using SqlSugar.IOC;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 依赖注入
builder.Services.AddScoped<ITypeinfoRepository, TypeInfoRepository>();
builder.Services.AddScoped<IWriteInfoRepository, WriteInfoRepository>();
builder.Services.AddScoped<IBaseNewsRepository, BlogNewsRepository>();

/*builder.Services.AddScoped<ITypeInfoServices, TypeInfoServices>();
builder.Services.AddScoped<IWriteInfoServices, WriteInfoServices>();
builder.Services.AddScoped<IBlogNewServices, BlogNewsServices>();*/

#endregion


#region 注入SqlSugar对象
builder.Services.AddSqlSugar(new IocConfig()
{
    ConnectionString = "server=.;Database=ZhurcMyBlog;Trusted_Connection=True;",
    DbType = IocDbType.SqlServer,
    IsAutoCloseConnection = true
});
builder.Services.ConfigurationSugar(db =>
{
    db.Aop.OnLogExecuting = (sql, p) =>
    {
        Console.WriteLine(sql);
    };
});
#endregion




var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



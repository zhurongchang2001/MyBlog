using MyBlog.IRepository;
using MyBlog.Repository;
using MyBlogIServices;
using MyBlogServices;
using SqlSugar.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region 依赖注入
builder.Services.AddScoped<IWriteInfoRepository, WriteInfoRepository>();
builder.Services.AddScoped<IWriteInfoServices, WriteInfoServices>();
#endregion

#region 注入SqlSugar对象
builder.Services.AddSqlSugar(new IocConfig()
{
    ConnectionString = "server=.;Database=ZhurcMyBlog;Trusted_Connection=True;",
    DbType = IocDbType.SqlServer,
    IsAutoCloseConnection = true
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

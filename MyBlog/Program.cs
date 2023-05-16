using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.IRepository;
using MyBlog.Repository;
using MyBlog.Utility;
using MyBlogIServices;
using MyBlogServices;
using SqlSugar;
using SqlSugar.IOC;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 依赖注入
builder.Services.AddScoped<ITypeinfoRepository, TypeInfoRepository>();
builder.Services.AddScoped<IWriteInfoRepository, WriteInfoRepository>();
builder.Services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();
builder.Services.AddScoped<ITypeInfoServices, TypeInfoServices>();
builder.Services.AddScoped<IWriteInfoServices, WriteInfoServices>();
builder.Services.AddScoped<IBlogNewServices, BlogNewsServices>();
#endregion

#region swagger 注释  + 鉴权  详情：https://blog.csdn.net/easyboot/article/details/129733043
builder.Services.AddSwaggerGen(option =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);

    #region 鉴权
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference=new OpenApiReference
              {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
              }
            },
            new string[] {}
          }
        });
    #endregion
});
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

#region JWT鉴权
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF")),
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:7026",
                    ValidateAudience = true,
                    ValidAudience = "http://localhost:7026",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(60)
                };
            });
#endregion

#region AutoMapper注入
//typeof(类)AutoMapper
builder.Services.AddAutoMapper(typeof(CustomAutoMapperProfile));
#endregion

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
//app.UseStaticFiles();//启用静态文件中间件，并将当前目录下的wwwroot文件夹作为默认的静态文件目录。
app.UseStaticFiles(new StaticFileOptions()
{
	FileProvider = new PhysicalFileProvider(
	Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
	RequestPath = "/Uploads"
});


/*//跨域配置
app.UseCors(MyAllowSpecificOrigins);*/

//UseAuthentication 就是鉴权 找出解析请求携带的信息
//UseAuthorization 授权 权限检查，看看有没有权限
app.UseHttpsRedirection();
//顺序一定不能错！！！！！！！！！
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();



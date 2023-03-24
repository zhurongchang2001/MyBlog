using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.IRepository;
using MyBlog.Repository;
using MyBlogIServices;
using MyBlogServices;
using SqlSugar;
using SqlSugar.IOC;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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
                    ValidAudience = "http://localhost:7037",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(60)
                };
            });
#endregion

/*#region 跨域配置
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:7037", "*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});
#endregion*/


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*//跨域配置
app.UseCors(MyAllowSpecificOrigins);*/
//授权
app.UseAuthorization();
//鉴权
app.UseAuthentication();

app.MapControllers();

app.Run();



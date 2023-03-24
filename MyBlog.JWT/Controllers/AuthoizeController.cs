using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Utility.ApiResult;
using MyBlogIServices;
using Md5Library;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyBlog.JWT.Controllers
{
    /// <summary>
    /// jwt鉴权
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthoizeController : ControllerBase
    {
        private readonly IWriteInfoServices _writeInfoServices;
        public AuthoizeController(IWriteInfoServices writeInfoServices) {
        this._writeInfoServices = writeInfoServices;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult>> Login(string username, string password)
        {
            string Md5Pwd = Md5Encryptor.Encrypt(password);
            var writeInfo=await _writeInfoServices.FindAsync(c => c.UserName == username && c.UserPwd== Md5Pwd);
            if (writeInfo!=null) {
                //登录成功
                var claims = new Claim[]
            {
                //类似于身份证，里面不能放置敏感信息，例如密码
                new Claim(ClaimTypes.Name, writeInfo.Name),
                new Claim("Id", writeInfo.Id.ToString()),
                new Claim("UserName", writeInfo.UserName)
            };
                //密钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                //issuer代表颁发Token的Web应用程序，audience是Token的受理者  路径 Properties=》launchSettings.json   applicationUrl
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:7026",
                    audience: "http://localhost:7037",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),//过期时间 1小时
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelp.Success(jwtToken,"登录成功");
            }
            else
            {
                return ApiResultHelp.Error("账号密码错误");
            }
        }
    }
}

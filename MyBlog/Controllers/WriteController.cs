using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Model;
using MyBlog.Utility.ApiResult;
using MyBlogIServices;
using System.Text;
using Md5Library;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.Controllers
{
    /// <summary>
    /// 作者
    /// </summary>
    [Route("Write/[controller]/[action]")]
    [ApiController]
    [Authorize]//鉴权

    public class WriteController : ControllerBase
    {
        private readonly IWriteInfoServices _WriteInfoServices;
        public WriteController(IWriteInfoServices writeInfoServices) {
        this._WriteInfoServices = writeInfoServices;
        }
        /// <summary>
        /// 新增作者
        /// </summary>
        /// <param name="Name">作者名称</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> AddWrite(string Name, string UserName,string Pwd) {
            //对用户名进行正则验证，只能允许大小写字母
            bool isValid = Regex.IsMatch(UserName, "^[a-zA-Z]+$");
            if (!isValid) {
                return ApiResultHelp.Error("用户名只能允许大小写字母");
            }

            //对密码进行正则验证，只能允许 同时包含字母和特殊字符
            bool isValidPwd = Regex.IsMatch(Pwd, @"^[a-zA-Z0-9]*[@#$%^&+?=][a-zA-Z0-9]*$");
            if (!isValidPwd)
            {
                return ApiResultHelp.Error("密码需字母和特殊字符");
            }
            //使用md5对密码进行加密操作
            string hashedPassword = Md5Encryptor.Encrypt(Pwd);
            WriteInfo writeInfo = new WriteInfo
            {
                Name = Name,
                UserName = UserName,
                UserPwd = hashedPassword
            };
            //判断要添加的数据，是否在数据库中存在
            var result = await _WriteInfoServices.FindAsync(c => c.UserName == UserName);
            if (result != null)
            {
                return ApiResultHelp.Error("账号已存在");
            }

            bool WriteBool = await _WriteInfoServices.CreateAsync(writeInfo);
            if (WriteBool)
            {
                return ApiResultHelp.Success(writeInfo, "添加");
            }
            else
            {
                return ApiResultHelp.Error("添加失败");
            }
        }
    }
}

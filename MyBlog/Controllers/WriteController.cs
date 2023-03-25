using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Model;
using MyBlog.Utility.ApiResult;
using MyBlogIServices;
using System.Text;
using Md5Library;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using MyBlog.Model.DTO;

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
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="OldPwd">旧密码</param>
        /// <param name="NewPwd">新密码</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> UpdateWrite(string OldPwd, string NewPwd) {
            //从jwt里拿取登录账户的id！！！！
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            /*//对密码进行正则验证，只能允许 同时包含字母和特殊字符
            bool isValidPwd = Regex.IsMatch(pwd, @"^[a-zA-Z0-9]*[@#$%^&+?=][a-zA-Z0-9]*$");
            if (!isValidPwd)
            {
                return ApiResultHelp.Error("密码需字母和特殊字符");
            }*/
            var result = await _WriteInfoServices.FindAsync(id);
            if (result.UserPwd!=Md5Encryptor.Encrypt(OldPwd)) {
                return ApiResultHelp.Error("密码错误，不允许修改");
            }
            result.UserPwd=Md5Encryptor.Encrypt(NewPwd);
            bool isValid=await _WriteInfoServices.UpdateAsync(result);
            if (isValid)
            {
                return ApiResultHelp.Success(result,"修改");
            }
            else
            {
                return ApiResultHelp.Error("修改失败");
            }
        }
        /// <summary>
        /// 重置 当前登录用户 密码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> ResetPwd() {
            //从jwt里拿取登录账户的id！！！！
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var result = await _WriteInfoServices.FindAsync(id);
            result.UserPwd = Md5Encryptor.Encrypt("123456");
            bool isValid = await _WriteInfoServices.UpdateAsync(result);
            if (isValid)
            {
                return ApiResultHelp.Success(result, "重置");
            }
            else
            {
                return ApiResultHelp.Error("重置失败");
            }
        }
        /// <summary>
        /// 使用AutoMapper查找，可以"过滤"敏感信息,相当于使用一个新映射
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
       // [AllowAnonymous]//这个可以让这个方法不鉴权，直接使用
        public async Task<ActionResult<ApiResult>> FindWrite([FromServices]IMapper mapper,int id) {
            //[FromServices]IMapper mapper 写在方法里的好处是，不用在控制器里面构造,只有调用方法时才使用
            var Write=await _WriteInfoServices.FindAsync(id);
            var WriteDto=mapper.Map<WriteInfoDTO>(Write);
            return ApiResultHelp.Success(WriteDto, "查找");
        }
    }
}

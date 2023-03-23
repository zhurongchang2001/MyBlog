using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IRepository;
using MyBlog.Model;
using MyBlog.Utility.ApiResult;
using MyBlogIServices;
using System.ComponentModel.Design;

namespace MyBlog.Controllers
{
    [Route("Type/[controller]/[action]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeinfoRepository _typeResolutionService;
        public TypeController (ITypeinfoRepository typeResolutionService) {
        this._typeResolutionService = typeResolutionService;
        }

        /// <summary>
        /// 类型添加
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult>> AddType(string TypeName) {
            /*var type = await _typeResolutionService.FindAsync(TypeName);
            if (type != null)
            {
                return ApiResultHelp.Error("该类型已存在");
            }*/
            if (string.IsNullOrEmpty(TypeName)) {
                return ApiResultHelp.Error("类型不能为空");
            }

            TypeInfo typeInfo = new TypeInfo
            {
                Name= TypeName
            };
            var typeBool = await _typeResolutionService.CreateAsync(typeInfo);
            if (typeBool)
            {
                return ApiResultHelp.Success(typeBool,"新增成功");
            }
            else {
                return ApiResultHelp.Error("新增失败");
            }
        }
        /// <summary>
        /// 类型删除
        /// </summary>
        /// <param name="TypeId">类型id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<ApiResult>> Delete(int TypeId) {
            var type = await _typeResolutionService.FindAsync(TypeId);
            if (type == null) {
                return ApiResultHelp.Error("该类型不存在");
            }
            var TypeBool = await _typeResolutionService.DeleteAsync(TypeId);
            if (TypeBool) {
                return ApiResultHelp.Success(TypeBool,"删除");
            }
            else
            {
                return ApiResultHelp.Error("删除失败");
            }

        }


    }
}

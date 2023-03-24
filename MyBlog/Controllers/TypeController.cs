using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IRepository;
using MyBlog.Model;
using MyBlog.Utility.ApiResult;
using MyBlogIServices;
using SqlSugar;
using System.ComponentModel.Design;

namespace MyBlog.Controllers
{
    /// <summary>
    /// 博客类型
    /// </summary>
    [Route("Type/[controller]/[action]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeInfoServices _typeResolutionService;
        public TypeController (ITypeInfoServices typeResolutionService) {
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
        /// <summary>
        /// 类型修改
        /// </summary>
        /// <param name="TypeId">类型id</param>
        /// <param name="TypeName">类型名称</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResult>> UpdateType(int TypeId,string TypeName) {
            var type =await _typeResolutionService.FindAsync(TypeId);
            if (type == null)
            {
                return ApiResultHelp.Error("该数据不存在");
            }
            type.Name = TypeName;
            var TypeBool = await _typeResolutionService.UpdateAsync(type);
            if (TypeBool) {
                return ApiResultHelp.Success(TypeBool,"修改");
            }
            else
            {
                return ApiResultHelp.Error("修改失败");
            }
        }

        /// <summary>
        /// 根据类型id查找类型
        /// </summary>
        /// <param name="TypeId">类型id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetType(int TypeId) {
            if (string.IsNullOrEmpty(TypeId.ToString()))
            {
                return ApiResultHelp.Error("类型id不能为空");
            }
            var TypeInfo = await _typeResolutionService.FindAsync(TypeId);
            if (TypeInfo == null)
            {
                return ApiResultHelp.Error("该条数据不存在");
            }
            else
            {
                return ApiResultHelp.Success(TypeInfo, "查找");
            }
        }
        /// <summary>
        /// 获取所有的类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResult>> GetAllType()
        {
            var TypeInfo = await _typeResolutionService.QueryAsync();
            if (TypeInfo == null)
            {
                return ApiResultHelp.Error("没有更多数据了");
            }
            else
            {
                return ApiResultHelp.Success(TypeInfo, "查找");
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string NoJWT() {
            return "没有授权";
        }
        [HttpGet]
        [Authorize]

        public string JWT()
        {
            return "有授权";
        }
    }
}

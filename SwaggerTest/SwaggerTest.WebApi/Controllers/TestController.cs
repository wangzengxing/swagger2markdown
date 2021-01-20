using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerTest.WebApi.Model;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerTest.WebApi.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 测试接口GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[ProducesResponseType(200, Type = typeof(Person))]
        public ApiResult<List<int>> Get()
        {
            return new ApiResult<List<int>>();
        }

        /// <summary>
        /// 测试接口POST
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            return Ok();
        }
    }
}

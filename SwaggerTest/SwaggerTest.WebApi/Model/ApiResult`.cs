using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerTest.WebApi.Model
{
    public class ApiResult<TData> where TData : class, new()
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public TData Data { get; set; } = new TData();
    }
}

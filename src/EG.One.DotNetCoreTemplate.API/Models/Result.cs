using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EG.One.DotNetCoreTemplate.Models
{
    public class Result<T>
    {
        public Result() { }
        public Result(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}

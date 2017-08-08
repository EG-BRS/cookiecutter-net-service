using AutoMapper;
using EG.One.DotNetCoreTemplate.Core.Responses;
using EG.One.DotNetCoreTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EG.One.DotNetCoreTemplate.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class DemoController : Controller
    {
        private IMapper _mapper;

        public DemoController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Demo
        /// </summary>
        /// <param text="text">the text to be returned</param>
        /// <returns>Returns a demo result</returns>
        /// <response code="200">Returns this always</response>
        [HttpGet("Text/{text}")]
        [ProducesResponseType(typeof(DemoResponse), 200)]
        public async Task<IActionResult> Text(string text)
        {
            // Mapper would normaly be used to map the response from some service into the DemoResponse
            var result = new Result<DemoResponse>(new DemoResponse() { Text = text });
            return Ok(result);
        }
    }
}

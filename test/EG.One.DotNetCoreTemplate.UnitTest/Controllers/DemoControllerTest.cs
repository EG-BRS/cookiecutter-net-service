using AutoMapper;
using EG.One.DotNetCoreTemplate.API.Controllers;
using EG.One.DotNetCoreTemplate.Core.Responses;
using EG.One.DotNetCoreTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace EG.One.DotNetCoreTemplate.UnitTest.Controllers
{
    public class DemoControllerTest
    {
        private DemoController _controller;
        private IMapper _mapper;

        public DemoControllerTest()
        {
            _mapper = Substitute.For<IMapper>();            

            _controller = new DemoController(_mapper);
        }

        [Fact]
        public async void GetDocuments_NullFromService_ReturnsNoContent()
        {
            IActionResult result = await _controller.Text("test");

            OkObjectResult objectResultResult = Assert.IsType<OkObjectResult>(result);
            Result<DemoResponse> dataResult = Assert.IsType<Result<DemoResponse>>(objectResultResult.Value);
            Assert.Equal("test", dataResult.Data.Text);
        }
    }
}

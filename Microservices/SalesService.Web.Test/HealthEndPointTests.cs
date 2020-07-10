using System.Net;
using System.Reflection.PortableExecutable;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using SalesService.Web;
using System.Threading.Tasks;
using FluentAssertions;

namespace SalesService.Web.Test
{
    public class FunctionalEndPoints: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public FunctionalEndPoints(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Fact]
        public async Task HealthEndpoint()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("/health");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task SwaggerEndpoint()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("/swagger");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

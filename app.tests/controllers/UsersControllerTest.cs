using app.controllers;
using app.controllers.dtos.users;
using app.domains.users;
using app.domains.users.service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace app.tests.controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsPaginatedListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "User1", Email = "user1@example.com" },
                new User { Id = 2, Name = "User2", Email = "user2@example.com" },
                new User { Id = 3, Name = "User3", Email = "user3@example.com" }
            };
            _mockUserService.Setup(service => service.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll(1, 2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<UserListResponse>(okResult.Value);
            Assert.Equal(2, response.Data.Count);
            Assert.Equal(3, response.Meta.Total);
            Assert.Equal(1, response.Meta.Page);
            Assert.Equal(2, response.Meta.PageSize);
        }
    }
}
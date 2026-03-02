using Microsoft.AspNetCore.Mvc;
using Moq;
using UserAuthApi.Controllers;
using UserAuthApi.DTOs;
using UserAuthApi.Interfaces;
using UserAuthApi.Models;
using Xunit;

namespace UserAuthApi.Tests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _mockService;

        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenEmailIsNew()
        {
            var request = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "Password123!"
            };

            var userResponse = new UserResponse
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            _mockService
                .Setup(s => s.RegisterAsync(request))
                .ReturnsAsync(userResponse);

            var result = await _controller.Register(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailExists()
        {
            var request = new RegisterRequest
            {
                Email = "john@example.com",
                Password = "Password123!"
            };

            _mockService
                .Setup(s => s.RegisterAsync(request))
                .ReturnsAsync((UserResponse?)null);

            var result = await _controller.Register(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email already exists.", badRequest.Value);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            var request = new LoginRequest
            {
                Email = "john@example.com",
                Password = "Password123!"
            };

            var userResponse = new UserResponse
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            var token = "fake-jwt-token";

            _mockService
                .Setup(s => s.LoginAsync(request))
                .ReturnsAsync((userResponse, token));

            var result = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            var request = new LoginRequest
            {
                Email = "john@example.com",
                Password = "WrongPassword!"
            };

            // Fixed: Removed invalid tuple cast
            _mockService
                .Setup(s => s.LoginAsync(request))
                .ReturnsAsync((null, null));

            var result = await _controller.Login(request);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials.", unauthorized.Value);
        }
    }
}
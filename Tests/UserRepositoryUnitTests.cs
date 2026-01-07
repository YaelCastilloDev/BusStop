using Domain.Entities;
using Infraestructur;
using Infraestructur.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Infraestructur.Models;
using Infraestructur.Identity.Models;

namespace Tests
{
    public class UserRepositoryUnitTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ApplicationDbContext _mockContext;
        private readonly UserRepository _repository;

        public UserRepositoryUnitTests()
        {
            // 1. Mock UserManager
            // We must mock IUserStore to create a functional mock UserManager
            var mockUserStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object, null, null, null, null, null, null, null, null);

            // 2. Mock DbContext
            // For these unit tests, we only need a simple in-memory context
            // to satisfy the constructor. It won't be used.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _mockContext = new ApplicationDbContext(options);

            // 3. Create the SUT (Subject Under Test)
            _repository = new UserRepository(_mockUserManager.Object, _mockContext);
        }

        [Fact]
        public async Task FindByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User { Id = Guid.NewGuid(), Email = email, UserName = "test" };

            _mockUserManager.Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _repository.FindByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Email, result.Email);
            _mockUserManager.Verify(m => m.FindByEmailAsync(email), Times.Once); // Verify it was called
        }

        [Fact]
        public async Task CreateAsync_ShouldCallUserManagerCreate_AndReturnSuccess()
        {
            // Arrange
            var user = new User { UserName = "newuser", Email = "new@example.com" };
            var password = "Password123!";
            var expectedResult = IdentityResult.Success;

            _mockUserManager.Setup(m => m.CreateAsync(user, password))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _repository.CreateAsync(user, password);

            // Assert
            Assert.True(result.Succeeded);
            _mockUserManager.Verify(m => m.CreateAsync(user, password), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnUser_WhenIdExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var expectedUser = new User { Id = Guid.Parse(userId), UserName = "testuser" };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _repository.FindByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id.ToString());
            _mockUserManager.Verify(m => m.FindByIdAsync(userId), Times.Once);
        }
    }
}
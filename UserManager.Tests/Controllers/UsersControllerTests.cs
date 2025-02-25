using UserManager.Controllers;
using UserManager.Data;
using UserManager.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using UserManager.Models.DTO;

namespace UserManager.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        [Test]
        public void GetUsers_ListOfUsersNotEmpty_ReturnsOk()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var storedUsers = new List<User>
            {
                new User 
                {
                    Id = 1,
                    Username = "TestUser",
                    Password = "hashedPass",
                    RoleId = 1,
                    Role = new Role 
                    { 
                        Id = 1,
                        Name = "TestRole",
                        Users = []
                    } 
                }
            };
            mockUserRepository.GetAllUsers().Returns(storedUsers);

            var result = controller.GetUsers() as OkObjectResult;
            Assert.That(result, Is.Not.Null, "Expected an OkObjectResult, but got null.");

            var returnedUsers = result.Value as List<UserDTO>;
            Assert.That(returnedUsers, Is.Not.Null, "Expected a List<UserDTO>, but got null.");
            Assert.That(returnedUsers.Count, Is.EqualTo(1));
            Assert.That(returnedUsers.FirstOrDefault().Id, Is.EqualTo(storedUsers.FirstOrDefault().Id));
        }

        [Test]
        public void GetUser_UserDoesNotExist_ReturnsNotFound()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            mockUserRepository.GetUserById(1).Returns((User?)null);

            var result = controller.GetUser(1) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a NotFoundObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void GetUser_UserExists_ReturnsUser()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var storedUser = new User
            {
                Id = 1,
                Username = "TestUser",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                {
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                } 
            };
            mockUserRepository.GetUserById(1).Returns(storedUser);

            var result = controller.GetUser(1) as OkObjectResult;
            Assert.That(result, Is.Not.Null, "Expected an OkObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var returnedUser = result.Value as UserDTO;
            Assert.That(returnedUser, Is.Not.Null, "Expected a UserDTO, but got null.");
            Assert.That(returnedUser.Id, Is.EqualTo(storedUser.Id));
            Assert.That(returnedUser.Username, Is.EqualTo(storedUser.Username));
        }

        [Test]
        public void CreateUser_WithValidUserData_ReturnsCreated()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var user = new User 
            {
                Id = 1,
                Username = "TestUser",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                { 
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                } 
            };

            var result = controller.CreateUser(user) as CreatedAtActionResult;
            mockUserRepository.Received(1).AddUser(Arg.Is<User>(u => u.Username == user.Username && u.Password == "hashedPass"));
            Assert.That(result, Is.Not.Null, "Expected a CreatedAtActionResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.ActionName, Is.EqualTo("GetUser"));

            var returnedUser = result.Value as User;
            Assert.That(returnedUser, Is.EqualTo(user));
        }

        [Test]
        public void CreateUser_WithInvalidUserData_ReturnsErrorMessage()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var user = new User 
            {
                Id = 1,
                Username = "TestUser",
                Email = "invalid-email",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                {
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                } 
            };
            controller.ModelState.AddModelError("Email", "The email is not valid.");
            
            var result = controller.CreateUser(user) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a BadRequestObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.InstanceOf<SerializableError>());
        }

        [Test]
        public void UpdateUser_UserDoesNotExist_ReturnsNotFound()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var updatedUser = new User 
            {
                Id = 1,
                Username = "TestUser",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                {
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                }
            };
            mockUserRepository.GetUserById(1).Returns((User?)null);

            var result = controller.UpdateUser(1, updatedUser) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a NotFoundObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void UpdateUser_UserExists_ReturnsUpdatedUser()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var user = new User 
            {
                Id = 1,
                Username = "TestUser",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                {
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                }
            };
            var updatedUser = new User 
            {
                Id = 1,
                Username = "UpdatedTestUser",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                {
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                }
            };
            mockUserRepository.GetUserById(1).Returns(user);

            var result = controller.UpdateUser(1, updatedUser) as NoContentResult;

            mockUserRepository.Received(1).UpdateUser(Arg.Is<User>(u => u.Id == 1 && u.Username == "UpdatedTestUser"));
            Assert.That(result, Is.Not.Null, "Expected a NoContentResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void DeleteUser_UserExists_CallsRepository()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            var storedUser = new User
            {
                Id = 1,
                Username = "TestUser",
                Password = "hashedPass",
                RoleId = 1,
                Role = new Role 
                { 
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                } 
            };
            mockUserRepository.GetUserById(1).Returns(storedUser);

            var result = controller.DeleteUser(1) as NoContentResult;

            mockUserRepository.Received(1).DeleteUser(1);
            Assert.That(result, Is.Not.Null, "Expected a NoContentResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void DeleteUser_UserDoesNotExist_ReturnsErrorMessage()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var controller = new UsersController(mockUserRepository);
            mockUserRepository.GetUserById(1).Returns((User?)null);

            var result = controller.DeleteUser(1) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a NotFoundResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(404));
            mockUserRepository.DidNotReceive().DeleteUser(1);
        }
    }
}

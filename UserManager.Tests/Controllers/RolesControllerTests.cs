using UserManager.Controllers;
using UserManager.Data;
using UserManager.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace UserManager.Tests.Controllers
{
    [TestFixture]
    public class RolesControllerTests
    {
        [Test]
        public void GetRoles_ListOfRolesNotEmpty_ReturnsOk()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var storedRoles = new List<Role>
            {
                new Role 
                { 
                    Id = 1,
                    Name = "TestRole",
                    Users = []
                } 
            };
            mockRoleRepository.GetAllRoles().Returns(storedRoles);

            var result = controller.GetRoles() as OkObjectResult;
            Assert.That(result, Is.Not.Null, "Expected an OkObjectResult, but got null.");

            var Roles = result.Value as List<Role>;
            Assert.That(Roles, Is.Not.Null, "Expected a list of Roles, but got null.");
            Assert.That(Roles.Count, Is.EqualTo(1));
            Assert.That(Roles.FirstOrDefault(), Is.EqualTo(storedRoles.FirstOrDefault()));
        }

        [Test]
        public void GetRole_RoleDoesNotExist_ReturnsNotFound()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            mockRoleRepository.GetRoleById(1).Returns((Role?)null);

            var result = controller.GetRole(1) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a NotFoundObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void GetRole_RoleExists_ReturnsRole()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var storedRole = new Role 
            { 
                Id = 1,
                Name = "TestRole",
                Users = []
            };
            mockRoleRepository.GetRoleById(1).Returns(storedRole);

            var result = controller.GetRole(1) as OkObjectResult;
            Assert.That(result, Is.Not.Null, "Expected an OkObjectResult, but got null.");

            var returnedRole = result.Value as Role;
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(returnedRole, Is.EqualTo(storedRole));
        }

        [Test]
        public void CreateRole_WithValidRoleData_ReturnsCreated()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var Role = new Role 
            { 
                Id = 1,
                Name = "TestRole",
                Users = []
            };

            var result = controller.CreateRole(Role) as CreatedAtActionResult;
            mockRoleRepository.Received(1).AddRole(Role);
            Assert.That(result, Is.Not.Null, "Expected a CreatedAtActionResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.ActionName, Is.EqualTo("GetRole"));

            var returnedRole = result.Value as Role;
            Assert.That(returnedRole, Is.EqualTo(Role));
        }

        [Test]
        public void CreateRole_WithInvalidRoleData_ReturnsErrorMessage()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var Role = new Role 
            {
                Id = 1,
                Name = "TestRole",
                Users = []
            };
            controller.ModelState.AddModelError("Email", "The email is not valid.");
            
            var result = controller.CreateRole(Role) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a BadRequestObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.InstanceOf<SerializableError>());
        }

        [Test]
        public void UpdateRole_RoleDoesNotExist_ReturnsNotFound()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var updatedRole = new Role
            {
                Id = 1,
                Name = "TestRole",
                Users = []
            };
            mockRoleRepository.GetRoleById(1).Returns((Role?)null);

            var result = controller.UpdateRole(1, updatedRole) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a NotFoundObjectResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void UpdateRole_RoleExists_ReturnsUpdatedRole()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var Role = new Role 
            {
                Id = 1,
                Name = "TestRole",
                Users = []
            };
            var updatedRole = new Role 
            {
                Id = 1,
                Name = "UpdatedTestRole",
                Users = []
            };
            mockRoleRepository.GetRoleById(1).Returns(Role);

            var result = controller.UpdateRole(1, updatedRole) as NoContentResult;

            mockRoleRepository.Received(1).UpdateRole(Arg.Is<Role>(r => r.Id == 1 && r.Name == "UpdatedTestRole"));
            Assert.That(result, Is.Not.Null, "Expected a NoContentResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void DeleteRole_RoleExists_CallsRepository()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            var storedRole = new Role
            {
                Id = 1,
                Name = "TestRole",
                Users = []
            };
            mockRoleRepository.GetRoleById(1).Returns(storedRole);

            var result = controller.DeleteRole(1) as NoContentResult;

            mockRoleRepository.Received(1).DeleteRole(1);
            Assert.That(result, Is.Not.Null, "Expected a NoContentResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void DeleteRole_RoleDoesNotExist_ReturnsErrorMessage()
        {
            var mockRoleRepository = Substitute.For<IRoleRepository>();
            var controller = new RolesController(mockRoleRepository);
            mockRoleRepository.GetRoleById(1).Returns((Role?)null);

            var result = controller.DeleteRole(1) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null, "Expected a NotFoundResult, but got null.");
            Assert.That(result.StatusCode, Is.EqualTo(404));
            mockRoleRepository.DidNotReceive().DeleteRole(1);
        }
    }
}
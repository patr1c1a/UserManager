using UserManager.Models;
using NSubstitute;
using static UserManager.Tests.Models.ModelValidation;

namespace UserManager.Tests.Models
{
    [TestFixture]
    public class RoleTests
    {
        [Test]
        public void Role_CreatedWithValidData_PassesValidation()
        {
            var role = new Role
            {
                Name = "ValidName",
                Users = []
            };

            var validationResults = ValidateModel(role);

            Assert.That(validationResults, Is.Empty, "Validation should pass for a valid role.");
        }

        [Test]
        public void Role_CreatedWithMissingName_FailsValidation()
        {
            var role = Substitute.For<Role>();
            role.Users = [];

            var validationResults = ValidateModel(role);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for missing name.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The role name is required."));
        }

        [Test]
        public void Role_CreatedWithMissingUsers_FailsValidation()
        {
            var role = Substitute.For<Role>();
            role.Name = "ValidName";

            var validationResults = ValidateModel(role);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for missing name.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The users list required (it can be empty)."));
        }
    }
}
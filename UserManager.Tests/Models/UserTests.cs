using UserManager.Models;
using System.ComponentModel.DataAnnotations;
using NSubstitute;

namespace UserManager.Tests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_CreatedWithValidData_PassesValidation()
        {
            var user = new User
            {
                Username = "ValidUsername",
                Password = "ValidPassword123!",
                RoleId = 1,
                Email = "valid@email.com"
            };

            var validationResults = ValidateModel(user);

            Assert.That(validationResults, Is.Empty, "Validation should pass for a valid user.");
        }

        [Test]
        public void User_CreatedWithMissingUsername_FailsValidation()
        {
            var user = Substitute.For<User>();
            user.Password = "ValidPassword123!";
            user.RoleId = 1;
            user.Email = "valid@email.com";

            var validationResults = ValidateModel(user);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for missing username.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The username is required."));
        }

        [Test]
        public void User_CreatedWithInvalidEmail_FailsValidation()
        {
            var user = new User
            {
                Username = "ValidUsername",
                Password = "ValidPassword123!",
                RoleId = 1,
                Email = "invalid-email"
            };

            var validationResults = ValidateModel(user);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for invalid email.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The email is not valid."));
        }

        [Test]
        public void User_CreatedWithShortUsername_FailsValidation()
        {
            var user = new User
            {
                Username = "abcd",
                Password = "ValidPassword123!",
                RoleId = 1,
                Email = "valid@email.com"
            };

            var validationResults = ValidateModel(user);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for short username.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The username must be between 5 and 30 characters."));
        }

        [Test]
        public void User_CreatedWithLongUsername_FailsValidation()
        {
            var user = new User
            {
                Username = new string('a', 31),
                Password = "ValidPassword123!",
                RoleId = 1,
                Email = "valid@email.com"
            };

            var validationResults = ValidateModel(user);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for long username.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The username must be between 5 and 30 characters."));
        }

        [Test]
        public void User_CreatedWithMissingPassword_FailsValidation()
        {
            var user = Substitute.For<User>();
            user.Username = "ValidUsername";
            user.RoleId = 1;
            user.Email = "valid@email.com";

            var validationResults = ValidateModel(user);

            Assert.That(validationResults, Has.Count.EqualTo(1), "Validation should fail for missing password.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The password is required."));
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
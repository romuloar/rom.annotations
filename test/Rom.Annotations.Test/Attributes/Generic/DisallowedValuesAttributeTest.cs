using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Generic
{
    public class DisallowedValuesAttributeTest
    {
        private class TestModel
        {
            [DisallowedValues("Admin", "Root", "SuperUser", ErrorMessage = "Username is not allowed")]
            public string Username { get; set; }
        }

        [Fact]
        public void Valid_WhenValueIsNotInDisallowedList()
        {
            var model = new TestModel { Username = "RegularUser" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Username) };
            var attr = new DisallowedValuesAttribute("Admin", "Root", "SuperUser")
            {
                ErrorMessage = "Username is not allowed"
            };

            var result = attr.GetValidationResult(model.Username, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenValueIsInDisallowedList()
        {
            var model = new TestModel { Username = "Admin" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Username) };
            var attr = new DisallowedValuesAttribute("Admin", "Root", "SuperUser")
            {
                ErrorMessage = "Username is not allowed"
            };

            var result = attr.GetValidationResult(model.Username, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Username is not allowed", result.ErrorMessage);
        }

        [Fact]
        public void Valid_WhenValueIsNull()
        {
            var model = new TestModel { Username = null };
            var context = new ValidationContext(model) { MemberName = nameof(model.Username) };
            var attr = new DisallowedValuesAttribute("Admin", "Root")
            {
                ErrorMessage = "Username is not allowed"
            };

            var result = attr.GetValidationResult(model.Username, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Valid_WhenValueIsEmpty()
        {
            var model = new TestModel { Username = string.Empty };
            var context = new ValidationContext(model) { MemberName = nameof(model.Username) };
            var attr = new DisallowedValuesAttribute("Admin", "Root")
            {
                ErrorMessage = "Username is not allowed"
            };

            var result = attr.GetValidationResult(model.Username, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

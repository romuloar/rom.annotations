using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Generic
{
    public class AllowedValuesAttributeTest
    {
        private class SampleModel
        {
            [AllowedValues("Admin", "User", "Guest", ErrorMessage = "Invalid role")]
            public string Role { get; set; }
        }

        [Fact]
        public void Valid_WhenValueIsInAllowedValues()
        {
            var model = new SampleModel { Role = "Admin" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Role) };
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateProperty(model.Role, context, results);

            Assert.True(isValid);
        }

        [Fact]
        public void Invalid_WhenValueIsNotInAllowedValues()
        {
            var model = new SampleModel { Role = "SuperUser" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Role) };
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateProperty(model.Role, context, results);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Invalid role");
        }

        [Fact]
        public void Valid_WhenValueIsNull()
        {
            var model = new SampleModel { Role = null };
            var context = new ValidationContext(model) { MemberName = nameof(model.Role) };
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateProperty(model.Role, context, results);

            Assert.True(isValid);
        }
    }
}

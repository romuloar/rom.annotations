using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.String
{
    public class StringContainsAttributeTest
    {
        private class TestModel
        {
            [StringContains("example", ErrorMessage = "Field must contain 'example'.")]
            public string Description { get; set; }
        }

        [Fact]
        public void Valid_WhenStringContainsSubstring()
        {
            var model = new TestModel { Description = "This is an example text." };
            var context = new ValidationContext(model) { MemberName = nameof(model.Description) };
            var attr = new StringContainsAttribute("example");

            var result = attr.GetValidationResult(model.Description, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenStringDoesNotContainSubstring()
        {
            var model = new TestModel { Description = "This is a test." };
            var context = new ValidationContext(model) { MemberName = nameof(model.Description) };
            var attr = new StringContainsAttribute("example") { ErrorMessage = "Missing substring" };

            var result = attr.GetValidationResult(model.Description, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Missing substring", result.ErrorMessage);
        }

        [Fact]
        public void Valid_WhenValueIsNull()
        {
            var attr = new StringContainsAttribute("example");
            var context = new ValidationContext(new object()) { MemberName = "Description" };

            var result = attr.GetValidationResult(null, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.String
{
    public class StringNotContainsAttributeTest
    {
        private class TestModel
        {
            [StringNotContains("forbidden", ErrorMessage = "Field must not contain 'forbidden'.")]
            public string Comment { get; set; }
        }

        [Fact]
        public void Valid_WhenStringDoesNotContainSubstring()
        {
            var model = new TestModel { Comment = "This is a safe comment." };
            var context = new ValidationContext(model) { MemberName = nameof(model.Comment) };
            var attr = new StringNotContainsAttribute("forbidden");

            var result = attr.GetValidationResult(model.Comment, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenStringContainsSubstring()
        {
            var model = new TestModel { Comment = "This contains a forbidden word." };
            var context = new ValidationContext(model) { MemberName = nameof(model.Comment) };
            var attr = new StringNotContainsAttribute("forbidden") { ErrorMessage = "Substring found" };

            var result = attr.GetValidationResult(model.Comment, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Substring found", result.ErrorMessage);
        }

        [Fact]
        public void Valid_WhenValueIsNull()
        {
            var attr = new StringNotContainsAttribute("forbidden");
            var context = new ValidationContext(new object()) { MemberName = "Comment" };

            var result = attr.GetValidationResult(null, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

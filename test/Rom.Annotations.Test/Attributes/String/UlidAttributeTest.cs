using Rom.Annotations.Attributes.String;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.String
{
    public class UlidAttributeTest
    {
        private class TestModel
        {
            [Ulid(ErrorMessage = "Invalid ULID")]
            public string Value { get; set; }
        }

        [Fact]
        public void IsValid_ReturnsSuccess_ForValidUlid()
        {
            var model = new TestModel { Value = "01ARZ3NDEKTSV4RRFFQ69G5FAV" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Value) };
            var attr = new UlidAttribute();

            var result = attr.GetValidationResult(model.Value, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FA")] // 25 chars
        [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAVX")] // 27 chars
        [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAV!")] // invalid char
        public void IsValid_ReturnsError_ForInvalidUlid(string value)
        {
            var model = new TestModel { Value = value };
            var context = new ValidationContext(model) { MemberName = nameof(model.Value) };
            var attr = new UlidAttribute { ErrorMessage = "Invalid ULID" };

            var result = attr.GetValidationResult(model.Value, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Invalid ULID", result.ErrorMessage);
        }
    }
}

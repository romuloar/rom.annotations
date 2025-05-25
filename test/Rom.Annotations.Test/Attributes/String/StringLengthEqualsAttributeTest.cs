using Rom.Annotations.Attributes.String;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.String
{
    public class StringLengthEqualsAttributeTest
    {
        private class TestModel
        {
            [StringLengthEquals(5)]
            public string Value { get; set; }
        }

        private ValidationContext GetContext(TestModel model)
        {
            return new ValidationContext(model, null, null)
            {
                MemberName = nameof(TestModel.Value)
            };
        }

        [Fact]
        public void IsValid_ReturnsSuccess_WhenStringHasExactLength()
        {
            var model = new TestModel { Value = "12345" };
            var attr = new StringLengthEqualsAttribute(5);
            var result = attr.GetValidationResult(model.Value, GetContext(model));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_ReturnsError_WhenStringIsShorter()
        {
            var model = new TestModel { Value = "123" };
            var attr = new StringLengthEqualsAttribute(5);
            var result = attr.GetValidationResult(model.Value, GetContext(model));
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The field Value must be exactly 5 characters long.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_ReturnsError_WhenStringIsLonger()
        {
            var model = new TestModel { Value = "1234567" };
            var attr = new StringLengthEqualsAttribute(5);
            var result = attr.GetValidationResult(model.Value, GetContext(model));
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The field Value must be exactly 5 characters long.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_ReturnsSuccess_WhenValueIsNull()
        {
            var model = new TestModel { Value = null };
            var attr = new StringLengthEqualsAttribute(5);
            var result = attr.GetValidationResult(model.Value, GetContext(model));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_ReturnsError_WhenValueIsNotString()
        {
            var attr = new StringLengthEqualsAttribute(5);
            var context = new ValidationContext(new object());
            var result = attr.GetValidationResult(12345, context);
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The value is not a string.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_UsesCustomErrorMessage()
        {
            var model = new TestModel { Value = "abc" };
            var attr = new StringLengthEqualsAttribute(5)
            {
                ErrorMessage = "Custom error"
            };
            var result = attr.GetValidationResult(model.Value, GetContext(model));
            Assert.Equal("Custom error", result.ErrorMessage);
        }
    }
}

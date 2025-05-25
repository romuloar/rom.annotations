using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.MultipleFields
{
    public class OnlyOneRequiredAttributeTest
    {
        private class TestModel
        {
            public string FieldA { get; set; }
            public string FieldB { get; set; }
            public string FieldC { get; set; }
        }

        private ValidationContext GetContext(TestModel model)
        {
            return new ValidationContext(model, null, null);
        }

        [Fact]
        public void IsValid_ReturnsSuccess_WhenExactlyOneFieldIsFilled()
        {
            var model = new TestModel { FieldA = "value" };
            var attr = new OnlyOneRequiredAttribute("FieldA", "FieldB", "FieldC");
            var result = attr.GetValidationResult(null, GetContext(model));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_ReturnsError_WhenNoFieldsAreFilled()
        {
            var model = new TestModel();
            var attr = new OnlyOneRequiredAttribute("FieldA", "FieldB", "FieldC");
            var result = attr.GetValidationResult(null, GetContext(model));
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Exactly one field must be filled.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_ReturnsError_WhenMoreThanOneFieldIsFilled()
        {
            var model = new TestModel { FieldA = "value", FieldB = "another" };
            var attr = new OnlyOneRequiredAttribute("FieldA", "FieldB", "FieldC");
            var result = attr.GetValidationResult(null, GetContext(model));
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Exactly one field must be filled.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_IgnoresWhitespaceStrings()
        {
            var model = new TestModel { FieldA = "   " };
            var attr = new OnlyOneRequiredAttribute("FieldA", "FieldB", "FieldC");
            var result = attr.GetValidationResult(null, GetContext(model));
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_UsesCustomErrorMessage()
        {
            var model = new TestModel();
            var attr = new OnlyOneRequiredAttribute("FieldA", "FieldB", "FieldC")
            {
                ErrorMessage = "Custom error"
            };
            var result = attr.GetValidationResult(null, GetContext(model));
            Assert.Equal("Custom error", result.ErrorMessage);
        }
    }
}

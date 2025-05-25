using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Numeric
{
    public class DecimalPrecisionAttributeTest
    {
        private class TestModel
        {
            [DecimalPrecision(2, ErrorMessage = "Value must have up to 2 decimal places.")]
            public decimal Value { get; set; }
        }

        [Fact]
        public void Valid_WhenDecimalPlacesAreWithinLimit()
        {
            var model = new TestModel { Value = 12.34m };
            var context = new ValidationContext(model) { MemberName = nameof(model.Value) };
            var attr = new DecimalPrecisionAttribute(2);

            var result = attr.GetValidationResult(model.Value, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Valid_WhenDecimalPlacesAreLessThanLimit()
        {
            var model = new TestModel { Value = 5m }; // no decimals
            var context = new ValidationContext(model) { MemberName = nameof(model.Value) };
            var attr = new DecimalPrecisionAttribute(2);

            var result = attr.GetValidationResult(model.Value, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenDecimalPlacesExceedLimit()
        {
            var model = new TestModel { Value = 12.345m };
            var context = new ValidationContext(model) { MemberName = nameof(model.Value) };
            var attr = new DecimalPrecisionAttribute(2) { ErrorMessage = "Too many decimals" };

            var result = attr.GetValidationResult(model.Value, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Too many decimals", result.ErrorMessage);
        }

        [Fact]
        public void Valid_WhenValueIsNull()
        {
            var attr = new DecimalPrecisionAttribute(2);
            var context = new ValidationContext(new object());

            var result = attr.GetValidationResult(null, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenValueIsNotDecimal()
        {
            var attr = new DecimalPrecisionAttribute(2) { ErrorMessage = "Invalid type" };
            var context = new ValidationContext(new object()) { MemberName = "Value" };

            var result = attr.GetValidationResult("not a decimal", context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Invalid type", result.ErrorMessage);
        }
    }
}

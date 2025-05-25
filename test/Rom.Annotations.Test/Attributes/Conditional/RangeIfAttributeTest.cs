using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RangeIfAttributeTest
    {
        private class TestModel
        {
            public string Status { get; set; }

            [RangeIf("Status", "Active", 10, 20, ErrorMessage = "Value must be between 10 and 20 when status is Active.")]
            public int Quantity { get; set; }
        }

        [Fact]
        public void Should_Pass_When_Status_NotMatch()
        {
            var model = new TestModel { Status = "Inactive", Quantity = 5 };
            var context = new ValidationContext(model) { MemberName = nameof(model.Quantity) };
            var attribute = new RangeIfAttribute("Status", "Active", 10, 20);

            var result = attribute.GetValidationResult(model.Quantity, context);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Should_Pass_When_Status_Match_And_Quantity_WithinRange()
        {
            var model = new TestModel { Status = "Active", Quantity = 15 };
            var context = new ValidationContext(model) { MemberName = nameof(model.Quantity) };
            var attribute = new RangeIfAttribute("Status", "Active", 10, 20);

            var result = attribute.GetValidationResult(model.Quantity, context);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Should_Fail_When_Status_Match_And_Quantity_OutsideRange()
        {
            var model = new TestModel { Status = "Active", Quantity = 25 };
            var context = new ValidationContext(model) { MemberName = nameof(model.Quantity) };
            var attribute = new RangeIfAttribute("Status", "Active", 10, 20) { ErrorMessage = "Value must be between 10 and 20 when status is Active." };

            var result = attribute.GetValidationResult(model.Quantity, context);
            Assert.NotNull(result);
            Assert.Equal("Value must be between 10 and 20 when status is Active.", result.ErrorMessage);
        }

        [Fact]
        public void Should_Pass_When_Value_Is_Null()
        {
            var model = new TestModel { Status = "Active", Quantity = 0 };
            var context = new ValidationContext(model) { MemberName = nameof(model.Quantity) };
            var attribute = new RangeIfAttribute("Status", "Active", 10, 20);

            // Test nullable scenario (simulate null by boxing)
            object nullValue = null;
            var result = attribute.GetValidationResult(nullValue, context);

            Assert.Equal(ValidationResult.Success, result);
        }

    }
}

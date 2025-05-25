using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class ConditionalPatternAttributeTest
    {
        private ValidationContext GetContext(object instance, string memberName) =>
            new ValidationContext(instance) { MemberName = memberName };

        private class TestModel
        {
            public string ConditionField { get; set; }

            [ConditionalPattern("ConditionField", "Yes", @"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid SSN format")]
            public string SSN { get; set; }
        }

        [Fact]
        public void Valid_WhenConditionNotMet()
        {
            var model = new TestModel
            {
                ConditionField = "No",
                SSN = "invalid"
            };

            var attr = new ConditionalPatternAttribute("ConditionField", "Yes", @"^\d{3}-\d{2}-\d{4}$");
            var result = attr.GetValidationResult(model.SSN, GetContext(model, nameof(model.SSN)));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Valid_WhenConditionMet_AndPatternMatches()
        {
            var model = new TestModel
            {
                ConditionField = "Yes",
                SSN = "123-45-6789"
            };

            var attr = new ConditionalPatternAttribute("ConditionField", "Yes", @"^\d{3}-\d{2}-\d{4}$");
            var result = attr.GetValidationResult(model.SSN, GetContext(model, nameof(model.SSN)));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenConditionMet_AndPatternDoesNotMatch()
        {
            var model = new TestModel
            {
                ConditionField = "Yes",
                SSN = "invalid-ssn"
            };
            var attr = new ConditionalPatternAttribute("ConditionField", "Yes", @"^\d{3}-\d{2}-\d{4}$");
            attr.ErrorMessage = "Invalid SSN format";

            var context = new ValidationContext(model) { MemberName = nameof(model.SSN) };
            var result = attr.GetValidationResult(model.SSN, context);
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Invalid SSN format", result.ErrorMessage);
        }

        [Fact]
        public void Invalid_WhenValueIsNotString()
        {
            var model = new
            {
                ConditionField = "Yes",
                SSN = (object)12345  // Not string
            };

            var attr = new ConditionalPatternAttribute("ConditionField", "Yes", @"^\d{3}-\d{2}-\d{4}$");
            attr.ErrorMessage = "Invalid SSN format";

            var context = new ValidationContext(model) { MemberName = "SSN" };
            var result = attr.GetValidationResult(model.SSN, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Invalid SSN format", result.ErrorMessage);
        }

        [Fact]
        public void Invalid_WhenOtherPropertyNotFound()
        {
            var model = new TestModel
            {
                ConditionField = "Yes",
                SSN = "123-45-6789"
            };

            var attr = new ConditionalPatternAttribute("NonExistentProperty", "Yes", @"^\d{3}-\d{2}-\d{4}$");
            var result = attr.GetValidationResult(model.SSN, GetContext(model, nameof(model.SSN)));

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Unknown property: NonExistentProperty", result.ErrorMessage);
        }
    }
}

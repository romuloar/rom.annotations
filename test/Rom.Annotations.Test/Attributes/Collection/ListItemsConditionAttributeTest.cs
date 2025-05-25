using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Collection
{
    public class ListItemsConditionAttributeTest
    {
        private ValidationResult Validate(object model, string propertyName)
        {
            var context = new ValidationContext(model) { MemberName = propertyName };
            var prop = model.GetType().GetProperty(propertyName);
            var value = prop.GetValue(model);
            var attrs = prop.GetCustomAttributes(typeof(ValidationAttribute), true) as ValidationAttribute[];
            foreach (var attr in attrs)
            {
                var result = attr.GetValidationResult(value, context);
                if (result != ValidationResult.Success) return result;
            }
            return ValidationResult.Success;
        }

        // Example: List of strings, all must be non-null/non-empty (RequiredStringAttribute)
        private class Model
        {
            [ListItemsCondition(typeof(RequiredStringAttribute))]
            public List<string> Tags { get; set; }
        }

        [Fact]
        public void Should_Fail_If_Any_Item_Is_Invalid()
        {
            var model = new Model { Tags = new List<string> { "valid", "", "also valid" } };
            var result = Validate(model, nameof(Model.Tags));
            result.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_All_Items_Are_Valid()
        {
            var model = new Model { Tags = new List<string> { "one", "two", "three" } };
            var result = Validate(model, nameof(Model.Tags));
            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_List_Is_Null()
        {
            var model = new Model { Tags = null };
            var result = Validate(model, nameof(Model.Tags));
            result.Should().Be(ValidationResult.Success);
        }

        // Custom RequiredStringAttribute for demonstration
        private class RequiredStringAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is string s && !string.IsNullOrWhiteSpace(s))
                    return ValidationResult.Success;
                return new ValidationResult("String must be non-empty.");
            }
        }
    }
}

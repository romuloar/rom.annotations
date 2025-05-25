using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Collection
{
    public class ListCountMinAttributeTest
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

        private class Model
        {
            [ListCountMin(2)]
            public List<int> Numbers { get; set; }
        }

        [Fact]
        public void Should_Fail_If_List_Has_Fewer_Than_Min()
        {
            var model = new Model { Numbers = new List<int> { 1 } };
            var result = Validate(model, nameof(Model.Numbers));
            result.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_List_Has_At_Least_Min()
        {
            var model = new Model { Numbers = new List<int> { 1, 2 } };
            var result = Validate(model, nameof(Model.Numbers));
            result.Should().Be(ValidationResult.Success);
        }
    }
}

using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Collection
{
    public class ListCountRangeAttributeTest
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
            [ListCountRange(2, 4)]
            public string[] Values { get; set; }
        }

        [Fact]
        public void Should_Fail_If_Less_Than_Min()
        {
            var model = new Model { Values = new[] { "x" } };
            var result = Validate(model, nameof(Model.Values));
            result.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Should_Fail_If_More_Than_Max()
        {
            var model = new Model { Values = new[] { "a", "b", "c", "d", "e" } };
            var result = Validate(model, nameof(Model.Values));
            result.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_Within_Range()
        {
            var model = new Model { Values = new[] { "a", "b", "c" } };
            var result = Validate(model, nameof(Model.Values));
            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_Null()
        {
            var model = new Model { Values = null };
            var result = Validate(model, nameof(Model.Values));
            result.Should().Be(ValidationResult.Success);
        }
    }
}

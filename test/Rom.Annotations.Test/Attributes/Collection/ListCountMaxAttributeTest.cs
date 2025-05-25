using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Collection
{
    public class ListCountMaxAttributeTest
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
            [ListCountMax(3)]
            public List<string> Items { get; set; }
        }

        [Fact]
        public void Should_Fail_If_List_Has_More_Than_Max()
        {
            var model = new Model { Items = new List<string> { "a", "b", "c", "d" } };
            var result = Validate(model, nameof(Model.Items));
            result.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_List_Has_At_Most_Max()
        {
            var model = new Model { Items = new List<string> { "a", "b", "c" } };
            var result = Validate(model, nameof(Model.Items));
            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_List_Is_Null()
        {
            var model = new Model { Items = null };
            var result = Validate(model, nameof(Model.Items));
            result.Should().Be(ValidationResult.Success);
        }
    }
}

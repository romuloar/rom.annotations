using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Collection
{
    public class ListItemsUniqueAttributeTest
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
            [ListItemsUnique]
            public List<int> Ids { get; set; }
        }

        [Fact]
        public void Should_Fail_If_List_Has_Duplicates()
        {
            var model = new Model { Ids = new List<int> { 1, 2, 2, 3 } };
            var result = Validate(model, nameof(Model.Ids));
            result.Should().NotBe(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_List_Is_Unique()
        {
            var model = new Model { Ids = new List<int> { 1, 2, 3 } };
            var result = Validate(model, nameof(Model.Ids));
            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void Should_Pass_If_List_Is_Null()
        {
            var model = new Model { Ids = null };
            var result = Validate(model, nameof(Model.Ids));
            result.Should().Be(ValidationResult.Success);
        }
    }
}

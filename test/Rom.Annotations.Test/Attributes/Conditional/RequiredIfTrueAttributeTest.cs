using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredIfTrueAttributeTest
    {
        private class TestModel
        {
            public bool IsActive { get; set; }
            [RequiredIfTrue("IsActive", ErrorMessage = "Name is required when IsActive is true")]
            public string Name { get; set; }

            public bool IsConfirmed { get; set; }

            [RequiredIfTrue(nameof(IsConfirmed), ErrorMessage = "Description is required if IsConfirmed is true.")]
            public string Description { get; set; }
        }
        private IList<ValidationResult> ValidateModel(TestModel model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
        [Fact]
        public void Should_Fail_Validation_When_Condition_Met_And_Field_Is_Empty()
        {
            var model = new TestModel { IsActive = true, Name = null };
            var results = ValidateModel(model);
            Assert.Single(results);
            Assert.Equal("Name is required when IsActive is true", results[0].ErrorMessage);
        }
        [Fact]
        public void Should_Pass_Validation_When_Condition_Met_And_Field_Has_Value()
        {
            var model = new TestModel { IsActive = true, Name = "John" };
            var results = ValidateModel(model);
            Assert.Empty(results);
        }
        [Fact]
        public void Should_Pass_Validation_When_Condition_Not_Met()
        {
            var model = new TestModel { IsActive = false, Name = null };
            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void Returns_ValidationError_If_IsConfirmedTrue_And_DescriptionNull()
        {
            var model = new TestModel { IsConfirmed = true, Description = null };

            var context = new ValidationContext(model) { MemberName = nameof(model.Description) };
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateProperty(model.Description, context, results);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("Description is required if IsConfirmed is true.", results[0].ErrorMessage);
        }

        [Fact]
        public void Returns_Success_If_IsConfirmedFalse()
        {
            var model = new TestModel { IsConfirmed = false, Description = null };

            var context = new ValidationContext(model) { MemberName = nameof(model.Description) };
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateProperty(model.Description, context, results);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Returns_Success_If_IsConfirmedTrue_And_DescriptionIsSet()
        {
            var model = new TestModel { IsConfirmed = true, Description = "Confirmed!" };

            var context = new ValidationContext(model) { MemberName = nameof(model.Description) };
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateProperty(model.Description, context, results);

            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}

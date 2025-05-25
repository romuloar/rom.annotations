using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredIfNullOrWhiteSpaceAttributeTest
    {
        private class SampleModel
        {
            public string AltEmail { get; set; }

            [RequiredIfNullOrWhiteSpace(nameof(AltEmail), ErrorMessage = "Email is required when AltEmail is missing.")]
            public string Email { get; set; }
        }

        [Fact]
        public void Should_Fail_Validation_When_Dependent_Is_Null_And_Field_Is_Null()
        {
            var model = new SampleModel
            {
                AltEmail = null,
                Email = null
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Email is required when AltEmail is missing.");
        }

        [Fact]
        public void Should_Pass_Validation_When_Dependent_Is_Not_Null()
        {
            var model = new SampleModel
            {
                AltEmail = "alt@example.com",
                Email = null
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.True(isValid);
        }

        [Fact]
        public void Should_Pass_Validation_When_Dependent_Is_Null_But_Field_Is_Provided()
        {
            var model = new SampleModel
            {
                AltEmail = null,
                Email = "test@example.com"
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.True(isValid);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredIfNotNullOrWhiteSpaceAttributeTest
    {
        private class TestModel
        {
            public string Dependent { get; set; }

            [RequiredIfNotNullOrWhiteSpace("Dependent", ErrorMessage = "Value is required when Dependent is not null")]
            public string Value { get; set; }
        }

        private List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, true);
            return results;
        }

        [Fact]
        public void Should_Require_Value_When_Dependent_Is_Not_Null()
        {
            var model = new TestModel
            {
                Dependent = "something",
                Value = null
            };

            var results = Validate(model);
            Assert.Single(results);
            Assert.Equal("Value is required when Dependent is not null", results[0].ErrorMessage);
        }

        [Fact]
        public void Should_Not_Require_Value_When_Dependent_Is_Null()
        {
            var model = new TestModel
            {
                Dependent = null,
                Value = null
            };

            var results = Validate(model);
            Assert.Empty(results);
        }

        [Fact]
        public void Should_Pass_When_Dependent_Is_Not_Null_And_Value_Is_Filled()
        {
            var model = new TestModel
            {
                Dependent = "exists",
                Value = "some value"
            };

            var results = Validate(model);
            Assert.Empty(results);
        }
    }
}

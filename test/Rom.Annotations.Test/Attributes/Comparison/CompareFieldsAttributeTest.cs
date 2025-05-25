using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Comparison
{
    public class CompareFieldsAttributeTest
    {
        private class TestModel
        {
            public string Field1 { get; set; }
            public string Field2 { get; set; }
        }

        [Fact]
        public void ReturnsSuccess_WhenFieldsAreEqual_AndMustBeEqualIsTrue()
        {
            var model = new TestModel { Field1 = "abc", Field2 = "abc" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Field1) };
            var attr = new CompareFieldsAttribute(nameof(model.Field2), mustBeEqual: true);

            var result = attr.GetValidationResult(model.Field1, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void ReturnsError_WhenFieldsAreNotEqual_AndMustBeEqualIsTrue()
        {
            var model = new TestModel { Field1 = "abc", Field2 = "def" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Field1) };
            var attr = new CompareFieldsAttribute(nameof(model.Field2), mustBeEqual: true)
            {
                ErrorMessage = "Fields must match"
            };

            var result = attr.GetValidationResult(model.Field1, context);

            Assert.NotNull(result);
            Assert.Equal("Fields must match", result.ErrorMessage);
        }

        [Fact]
        public void ReturnsSuccess_WhenFieldsAreDifferent_AndMustBeEqualIsFalse()
        {
            var model = new TestModel { Field1 = "abc", Field2 = "def" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Field1) };
            var attr = new CompareFieldsAttribute(nameof(model.Field2), mustBeEqual: false);

            var result = attr.GetValidationResult(model.Field1, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void ReturnsError_WhenFieldsAreEqual_AndMustBeEqualIsFalse()
        {
            var model = new TestModel { Field1 = "abc", Field2 = "abc" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Field1) };
            var attr = new CompareFieldsAttribute(nameof(model.Field2), mustBeEqual: false)
            {
                ErrorMessage = "Fields must differ"
            };

            var result = attr.GetValidationResult(model.Field1, context);

            Assert.NotNull(result);
            Assert.Equal("Fields must differ", result.ErrorMessage);
        }
    }
}

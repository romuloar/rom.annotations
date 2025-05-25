using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Comparison
{
    public class NotEqualToAttributeTest
    {
        private class TestClass
        {
            public string FieldA { get; set; }

            [NotEqualTo("FieldA", ErrorMessage = "FieldB should not be equal to FieldA")]
            public string FieldB { get; set; }
        }

        [Fact]
        public void IsValid_ReturnsSuccess_WhenValuesAreDifferent()
        {
            var obj = new TestClass { FieldA = "Test1", FieldB = "Test2" };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.FieldB) };
            var attr = new NotEqualToAttribute("FieldA");

            var result = attr.GetValidationResult(obj.FieldB, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_ReturnsError_WhenValuesAreEqual()
        {
            var obj = new TestClass { FieldA = "SameValue", FieldB = "SameValue" };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.FieldB) };
            var attr = new NotEqualToAttribute("FieldA") { ErrorMessage = "Fields must not be equal" };

            var result = attr.GetValidationResult(obj.FieldB, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Fields must not be equal", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_ReturnsSuccess_WhenBothAreNull()
        {
            var obj = new TestClass { FieldA = null, FieldB = "Value" };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.FieldB) };
            var attr = new NotEqualToAttribute("FieldA");

            var result = attr.GetValidationResult(obj.FieldB, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Comparison
{
    public class GreaterThanAttributeTest
    {
        private class TestClass
        {
            public int Start { get; set; }

            [GreaterThan("Start", ErrorMessage = "End must be greater than Start")]
            public int End { get; set; }
        }

        [Fact]
        public void Valid_When_EndIsGreaterThanStart()
        {
            var obj = new TestClass { Start = 5, End = 10 };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.End) };
            var attr = new GreaterThanAttribute("Start");

            var result = attr.GetValidationResult(obj.End, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_When_EndIsEqualToStart()
        {
            var obj = new TestClass { Start = 10, End = 10 };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.End) };
            var attr = new GreaterThanAttribute("Start") { ErrorMessage = "End must be greater" };

            var result = attr.GetValidationResult(obj.End, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("End must be greater", result.ErrorMessage);
        }

        [Fact]
        public void Invalid_When_EndIsLessThanStart()
        {
            var obj = new TestClass { Start = 15, End = 10 };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.End) };
            var attr = new GreaterThanAttribute("Start");

            var result = attr.GetValidationResult(obj.End, context);

            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void Valid_When_NullValues()
        {
            var obj = new TestClass { Start = 0, End = 0 };
            var context = new ValidationContext(obj) { MemberName = nameof(obj.End) };
            var attr = new GreaterThanAttribute("Start");

            // Testing with nulls
            var result = attr.GetValidationResult(null, context);
            Assert.Equal(ValidationResult.Success, result);

            var result2 = attr.GetValidationResult(obj.End, new ValidationContext(obj) { MemberName = nameof(obj.End) });
            Assert.NotEqual(ValidationResult.Success, result2);
        }
    }
}

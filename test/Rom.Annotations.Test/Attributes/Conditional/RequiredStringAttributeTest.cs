using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredStringAttributeTest
    {
        private class TestClass
        {
            [RequiredString(ErrorMessage = "The Name field must not be empty or whitespace.")]
            public string Name { get; set; }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void IsInvalid_WhenStringIsNullOrWhitespace_ReturnsError(string value)
        {
            var testObj = new TestClass { Name = value };
            var context = new ValidationContext(testObj) { MemberName = nameof(TestClass.Name) };
            var attribute = new RequiredStringAttribute { ErrorMessage = "The Name field must not be empty or whitespace." };

            var result = attribute.GetValidationResult(testObj.Name, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The Name field must not be empty or whitespace.", result.ErrorMessage);
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("Valid String")]
        [InlineData("123")]
        public void IsValid_WhenStringHasContent_ReturnsSuccess(string value)
        {
            var testObj = new TestClass { Name = value };
            var context = new ValidationContext(testObj) { MemberName = nameof(TestClass.Name) };
            var attribute = new RequiredStringAttribute();

            var result = attribute.GetValidationResult(testObj.Name, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

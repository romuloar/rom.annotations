using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredGuidAttributeTest
    {
        private class TestClass
        {
            [RequiredGuid(ErrorMessage = "The Id field must not be empty.")]
            public Guid Id { get; set; }
        }

        [Fact]
        public void IsInvalid_WhenGuidIsEmpty_ReturnsError()
        {
            var testObj = new TestClass { Id = Guid.Empty };
            var context = new ValidationContext(testObj) { MemberName = nameof(TestClass.Id) };
            var attribute = new RequiredGuidAttribute { ErrorMessage = "The Id field must not be empty." };

            var result = attribute.GetValidationResult(testObj.Id, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The Id field must not be empty.", result.ErrorMessage);
        }

        [Fact]
        public void IsInvalid_WhenGuidIsNull_ReturnsError()
        {
            var testObj = new TestClass { Id = Guid.Empty }; // Guid não pode ser null, mas se fosse Guid? testaria null
            var context = new ValidationContext(testObj) { MemberName = nameof(TestClass.Id) };
            var attribute = new RequiredGuidAttribute { ErrorMessage = "The Id field must not be empty." };

            var result = attribute.GetValidationResult(null, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The Id field must not be empty.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenGuidIsNotEmpty_ReturnsSuccess()
        {
            var testObj = new TestClass { Id = Guid.NewGuid() };
            var context = new ValidationContext(testObj) { MemberName = nameof(TestClass.Id) };
            var attribute = new RequiredGuidAttribute();

            var result = attribute.GetValidationResult(testObj.Id, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

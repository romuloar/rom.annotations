using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredIfInSetAttributeTest
    {
        private class TestModel
        {
            public string? Status { get; set; }

            [RequiredIfInSet("Status", "Pending", "InProgress", ErrorMessage = "IsActive is required if Status is Pending or InProgress")]
            public string? IsActive { get; set; }
        }

        [Fact]
        public void IsValid_Should_BeValid_When_OtherPropertyValueNotInSet()
        {
            var model = new TestModel
            {
                Status = "Completed",
                IsActive = null
            };

            var context = new ValidationContext(model) { MemberName = nameof(model.IsActive) };
            var attribute = new RequiredIfInSetAttribute("Status", "Pending", "InProgress");

            var result = attribute.GetValidationResult(model.IsActive, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Should_BeInvalid_When_OtherPropertyValueInSet_And_FieldIsNull()
        {
            var model = new TestModel
            {
                Status = "Pending",
                IsActive = null
            };

            var context = new ValidationContext(model) { MemberName = nameof(model.IsActive) };
            var attribute = new RequiredIfInSetAttribute("Status", "Pending", "InProgress")
            {
                ErrorMessage = "IsActive is required"
            };

            var result = attribute.GetValidationResult(model.IsActive, context);

            Assert.NotNull(result);
            Assert.Equal("IsActive is required", result?.ErrorMessage);
        }

        [Fact]
        public void IsValid_Should_BeInvalid_When_OtherPropertyValueInSet_And_FieldIsEmptyString()
        {
            var model = new TestModel
            {
                Status = "InProgress",
                IsActive = "   "
            };

            var context = new ValidationContext(model) { MemberName = nameof(model.IsActive) };
            var attribute = new RequiredIfInSetAttribute("Status", "Pending", "InProgress")
            {
                ErrorMessage = "IsActive is required"
            };

            var result = attribute.GetValidationResult(model.IsActive, context);

            Assert.NotNull(result);
            Assert.Equal("IsActive is required", result?.ErrorMessage);
        }

        [Fact]
        public void IsValid_Should_BeValid_When_OtherPropertyValueInSet_And_FieldIsSet()
        {
            var model = new TestModel
            {
                Status = "Pending",
                IsActive = "Yes"
            };

            var context = new ValidationContext(model) { MemberName = nameof(model.IsActive) };
            var attribute = new RequiredIfInSetAttribute("Status", "Pending", "InProgress");

            var result = attribute.GetValidationResult(model.IsActive, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredIfFalseAttributeTest
    {
        private class TestModel
        {
            public bool IsActive { get; set; }

            [RequiredIfFalse("IsActive", ErrorMessage = "Reason is required when IsActive is false.")]
            public string Reason { get; set; }
        }

        [Fact]
        public void Should_Fail_When_IsActive_False_And_Reason_Null()
        {
            var model = new TestModel { IsActive = false, Reason = null };
            var context = new ValidationContext(model) { MemberName = nameof(model.Reason) };
            var attribute = new RequiredIfFalseAttribute("IsActive") { ErrorMessage = "Reason is required when IsActive is false." };

            var result = attribute.GetValidationResult(model.Reason, context);

            Assert.NotNull(result);
            Assert.Equal("Reason is required when IsActive is false.", result.ErrorMessage);
        }

        [Fact]
        public void Should_Fail_When_IsActive_False_And_Reason_Empty()
        {
            var model = new TestModel { IsActive = false, Reason = "  " };
            var context = new ValidationContext(model) { MemberName = nameof(model.Reason) };
            var attribute = new RequiredIfFalseAttribute("IsActive") { ErrorMessage = "Reason is required when IsActive is false." };

            var result = attribute.GetValidationResult(model.Reason, context);

            Assert.NotNull(result);
            Assert.Equal("Reason is required when IsActive is false.", result.ErrorMessage);
        }

        [Fact]
        public void Should_Pass_When_IsActive_True_And_Reason_Null()
        {
            var model = new TestModel { IsActive = true, Reason = null };
            var context = new ValidationContext(model) { MemberName = nameof(model.Reason) };
            var attribute = new RequiredIfFalseAttribute("IsActive");

            var result = attribute.GetValidationResult(model.Reason, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Should_Pass_When_IsActive_False_And_Reason_NotEmpty()
        {
            var model = new TestModel { IsActive = false, Reason = "Some reason" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Reason) };
            var attribute = new RequiredIfFalseAttribute("IsActive");

            var result = attribute.GetValidationResult(model.Reason, context);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

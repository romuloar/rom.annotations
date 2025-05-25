using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public enum Status
    {
        None = 0,
        Active = 1,
        Inactive = 2
    }

    public class RequiredEnumAttributeTest
    {
        private class TestModel
        {
            [RequiredAttributeEnum(ErrorMessage = "Status must be selected.")]
            public Status CurrentStatus { get; set; }
        }

        [Fact]
        public void Valid_WhenEnumIsNotDefault()
        {
            var model = new TestModel { CurrentStatus = Status.Active };
            var context = new ValidationContext(model) { MemberName = nameof(model.CurrentStatus) };
            var attr = new RequiredAttributeEnum();

            var result = attr.GetValidationResult(model.CurrentStatus, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_WhenEnumIsDefault()
        {
            var model = new TestModel { CurrentStatus = Status.None };
            var context = new ValidationContext(model) { MemberName = nameof(model.CurrentStatus) };
            var attr = new RequiredAttributeEnum() { ErrorMessage = "Must select a status" };

            var result = attr.GetValidationResult(model.CurrentStatus, context);

            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Must select a status", result.ErrorMessage);
        }
    }
}

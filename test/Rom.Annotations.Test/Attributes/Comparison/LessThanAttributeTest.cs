using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Comparison
{
    public class LessThanAttributeTest
    {
        private ValidationContext GetContext(object instance, string memberName)
        {
            return new ValidationContext(instance)
            {
                MemberName = memberName
            };
        }

        class NumericModel
        {
            public int MaxValue { get; set; }

            [LessThan("MaxValue", ErrorMessage = "MinValue must be less than MaxValue")]
            public int MinValue { get; set; }
        }

        class DateModel
        {
            public DateTime EndDate { get; set; }

            [LessThan("EndDate")]
            public DateTime StartDate { get; set; }
        }

        [Fact]
        public void Numeric_Valid_When_MinLessThanMax()
        {
            var model = new NumericModel { MaxValue = 10, MinValue = 5 };
            var attr = new LessThanAttribute("MaxValue");
            var result = attr.GetValidationResult(model.MinValue, GetContext(model, nameof(model.MinValue)));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Numeric_Invalid_When_MinNotLessThanMax()
        {
            var model = new NumericModel { MaxValue = 10, MinValue = 15 };
            var attr = new LessThanAttribute("MaxValue") { ErrorMessage = "MinValue must be less than MaxValue" };
            var result = attr.GetValidationResult(model.MinValue, GetContext(model, nameof(model.MinValue)));
            Assert.NotNull(result);
            Assert.Equal("MinValue must be less than MaxValue", result.ErrorMessage);
        }

        [Fact]
        public void DateTime_Valid_When_StartBeforeEnd()
        {
            var model = new DateModel
            {
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };
            var attr = new LessThanAttribute("EndDate");
            var result = attr.GetValidationResult(model.StartDate, GetContext(model, nameof(model.StartDate)));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void DateTime_Invalid_When_StartNotBeforeEnd()
        {
            var model = new DateModel
            {
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };
            var attr = new LessThanAttribute("EndDate") { ErrorMessage = "StartDate must be less than EndDate" };
            var result = attr.GetValidationResult(model.StartDate, GetContext(model, nameof(model.StartDate)));
            Assert.NotNull(result);
            Assert.Equal("StartDate must be less than EndDate", result.ErrorMessage);
        }

        [Fact]
        public void NullValue_IsValid()
        {
            var model = new NumericModel { MaxValue = 10, MinValue = 0 };
            var attr = new LessThanAttribute("MaxValue");
            var result = attr.GetValidationResult(null, GetContext(model, nameof(model.MinValue)));
            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Date
{
    public class DateLaterThanAttributeTest
    {
        private class TestModel
        {
            public DateTime StartDate { get; set; }

            [DateLaterThan("StartDate", ErrorMessage = "EndDate must be later than StartDate")]
            public DateTime EndDate { get; set; }
        }

        [Fact]
        public void Valid_When_EndDateIsLater()
        {
            var model = new TestModel
            {
                StartDate = new DateTime(2024, 5, 24),
                EndDate = new DateTime(2024, 5, 25)
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) };
            var attribute = new DateLaterThanAttribute("StartDate");

            var result = attribute.GetValidationResult(model.EndDate, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_When_EndDateIsNotLater()
        {
            var model = new TestModel
            {
                StartDate = new DateTime(2024, 5, 24),
                EndDate = new DateTime(2024, 5, 23)
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) };
            var attribute = new DateLaterThanAttribute("StartDate") { ErrorMessage = "EndDate must be later than StartDate" };

            var result = attribute.GetValidationResult(model.EndDate, context);

            Assert.NotNull(result);
            Assert.Equal("EndDate must be later than StartDate", result.ErrorMessage);
        }

        [Fact]
        public void Valid_When_OtherPropertyIsNull()
        {
            var model = new TestModel
            {
                StartDate = default,
                EndDate = new DateTime(2024, 5, 25)
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) };
            var attribute = new DateLaterThanAttribute("StartDate");

            var result = attribute.GetValidationResult(model.EndDate, context);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_When_PropertyDoesNotExist()
        {
            var model = new TestModel
            {
                StartDate = new DateTime(2024, 5, 24),
                EndDate = new DateTime(2024, 5, 25)
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.EndDate) };
            var attribute = new DateLaterThanAttribute("NonExistentProperty");

            var result = attribute.GetValidationResult(model.EndDate, context);

            Assert.NotNull(result);
            Assert.Equal("Unknown property: NonExistentProperty", result.ErrorMessage);
        }
    }
}

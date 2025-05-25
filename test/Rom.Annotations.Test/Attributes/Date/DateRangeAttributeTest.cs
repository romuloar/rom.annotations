using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Date
{
    public class DateRangeAttributeTest
    {
        // Test class to simulate the object being validated
        private class TestModel
        {
            [DateRange("2023-01-01", "2023-12-31")]
            public DateTime EventDate { get; set; }

            [DateRange("2020-01-01", "2025-12-31", ErrorMessage = "Birth date must be within the specified range")]
            public DateTime BirthDate { get; set; }

            [DateRange("2023-06-01", "2023-06-30")]
            public DateTime? OptionalDate { get; set; }

            [DateRange("2023-01-01 09:00:00", "2023-01-01 17:00:00")]
            public DateTime AppointmentTime { get; set; }
        }

        [Fact]
        public void Valid_When_DateIsInRange()
        {
            var model = new TestModel { EventDate = new DateTime(2024, 6, 15) };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.EventDate) };
            var attribute = new DateRangeAttribute("2024-01-01", "2024-12-31");

            var result = attribute.GetValidationResult(model.EventDate, context);

            Assert.Equal(ValidationResult.Success, result);
        }
        [Fact]
        public void Constructor_WithValidDates_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Assert
            Assert.Equal(new DateTime(2023, 1, 1), attribute.MinDate);
            Assert.Equal(new DateTime(2023, 12, 31), attribute.MaxDate);
        }

        [Fact]
        public void Constructor_WithInvalidMinDate_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new DateRangeAttribute("invalid-date", "2023-12-31"));

            Assert.Equal("Invalid minDate format (Parameter 'minDate')", exception.Message);
        }

        [Fact]
        public void Constructor_WithInvalidMaxDate_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new DateRangeAttribute("2023-01-01", "invalid-date"));

            Assert.Equal("Invalid maxDate format (Parameter 'maxDate')", exception.Message);
        }

        [Fact]
        public void Constructor_WithMinDateGreaterThanMaxDate_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new DateRangeAttribute("2023-12-31", "2023-01-01"));

            Assert.Equal("minDate must be less than or equal to maxDate", exception.Message);
        }

        [Fact]
        public void Constructor_WithEqualMinAndMaxDates_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new DateRangeAttribute("2023-06-15", "2023-06-15");

            // Assert
            Assert.Equal(new DateTime(2023, 6, 15), attribute.MinDate);
            Assert.Equal(new DateTime(2023, 6, 15), attribute.MaxDate);
        }

        [Fact]
        public void IsValid_WithDateInRange_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { EventDate = new DateTime(2023, 6, 15) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.EventDate),
                DisplayName = "Event Date"
            };
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var result = attribute.GetValidationResult(model.EventDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithDateAtMinBoundary_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { EventDate = new DateTime(2023, 1, 1) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.EventDate),
                DisplayName = "Event Date"
            };
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var result = attribute.GetValidationResult(model.EventDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithDateAtMaxBoundary_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { EventDate = new DateTime(2023, 12, 31) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.EventDate),
                DisplayName = "Event Date"
            };
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var result = attribute.GetValidationResult(model.EventDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithDateBelowMinRange_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel { EventDate = new DateTime(2022, 12, 31) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.EventDate),
                DisplayName = "Event Date"
            };
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var result = attribute.GetValidationResult(model.EventDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Event Date must be between 2023-01-01 and 2023-12-31 (inclusive).", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithDateAboveMaxRange_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel { EventDate = new DateTime(2024, 1, 1) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.EventDate),
                DisplayName = "Event Date"
            };
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var result = attribute.GetValidationResult(model.EventDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Event Date must be between 2023-01-01 and 2023-12-31 (inclusive).", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var model = new TestModel { BirthDate = new DateTime(2019, 12, 31) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.BirthDate),
                DisplayName = "Birth Date"
            };
            var attribute = new DateRangeAttribute("2020-01-01", "2025-12-31")
            {
                ErrorMessage = "Birth date must be within the specified range"
            };

            // Act
            var result = attribute.GetValidationResult(model.BirthDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Birth date must be within the specified range", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNullValue_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { OptionalDate = null };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalDate),
                DisplayName = "Optional Date"
            };
            var attribute = new DateRangeAttribute("2023-06-01", "2023-06-30");

            // Act
            var result = attribute.GetValidationResult(model.OptionalDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNonDateTimeValue_ReturnsTypeError()
        {
            // Arrange
            var context = new ValidationContext(new object())
            {
                MemberName = "TestProperty",
                DisplayName = "Test Property"
            };
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var result = attribute.GetValidationResult("not a date", context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("The field must be a valid DateTime.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithTimeComponent_InRange_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { AppointmentTime = new DateTime(2023, 1, 1, 12, 0, 0) }; // 12:00 PM
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.AppointmentTime),
                DisplayName = "Appointment Time"
            };
            var attribute = new DateRangeAttribute("2023-01-01 09:00:00", "2023-01-01 17:00:00");

            // Act
            var result = attribute.GetValidationResult(model.AppointmentTime, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithTimeComponent_OutOfRange_ReturnsError()
        {
            // Arrange
            var model = new TestModel { AppointmentTime = new DateTime(2023, 1, 1, 18, 0, 0) }; // 6:00 PM
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.AppointmentTime),
                DisplayName = "Appointment Time"
            };
            var attribute = new DateRangeAttribute("2023-01-01 09:00:00", "2023-01-01 17:00:00");

            // Act
            var result = attribute.GetValidationResult(model.AppointmentTime, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Contains("must be between", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNullableDateTime_InRange_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { OptionalDate = new DateTime(2023, 6, 15) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalDate),
                DisplayName = "Optional Date"
            };
            var attribute = new DateRangeAttribute("2023-06-01", "2023-06-30");

            // Act
            var result = attribute.GetValidationResult(model.OptionalDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNullableDateTime_OutOfRange_ReturnsError()
        {
            // Arrange
            var model = new TestModel { OptionalDate = new DateTime(2023, 7, 1) };
            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalDate),
                DisplayName = "Optional Date"
            };
            var attribute = new DateRangeAttribute("2023-06-01", "2023-06-30");

            // Act
            var result = attribute.GetValidationResult(model.OptionalDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Optional Date must be between 2023-06-01 and 2023-06-30 (inclusive).", result.ErrorMessage);
        }

        [Fact]
        public void FormatErrorMessage_WithDefaultMessage_ReturnsFormattedMessage()
        {
            // Arrange
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31");

            // Act
            var message = attribute.FormatErrorMessage("Test Field");

            // Assert
            Assert.Equal("Test Field must be between 2023-01-01 and 2023-12-31 (inclusive).", message);
        }

        [Fact]
        public void FormatErrorMessage_WithCustomMessage_ReturnsCustomMessage()
        {
            // Arrange
            var attribute = new DateRangeAttribute("2023-01-01", "2023-12-31")
            {
                ErrorMessage = "Custom error message"
            };

            // Act
            var message = attribute.FormatErrorMessage("Test Field");

            // Assert
            Assert.Equal("Custom error message", message);
        }
 
        [Fact]
        public void Constructor_WithDifferentDateFormats_ParsesCorrectly()
        {
            // Arrange & Act
            var attribute1 = new DateRangeAttribute("01/01/2023", "12/31/2023");
            var attribute2 = new DateRangeAttribute("2023-06-15T10:30:00", "2023-06-15T15:45:00");

            // Assert
            Assert.Equal(new DateTime(2023, 1, 1), attribute1.MinDate);
            Assert.Equal(new DateTime(2023, 12, 31), attribute1.MaxDate);
            Assert.Equal(new DateTime(2023, 6, 15, 10, 30, 0), attribute2.MinDate);
            Assert.Equal(new DateTime(2023, 6, 15, 15, 45, 0), attribute2.MaxDate);
        }
    }
}

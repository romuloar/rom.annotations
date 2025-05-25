using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Date
{
    public class DateEarlierThanAttributeTest
    {
        private ValidationContext GetContext(object instance, string memberName)
        {
            return new ValidationContext(instance) { MemberName = memberName };
        }

        private class SampleModel
        {
            public DateTime EndDate { get; set; }

            [DateEarlierThan("EndDate", ErrorMessage = "StartDate must be earlier than EndDate")]
            public DateTime StartDate { get; set; }
        }

        [Fact]
        public void Valid_When_StartDateIsEarlier()
        {
            var model = new SampleModel
            {
                StartDate = new DateTime(2023, 01, 01),
                EndDate = new DateTime(2023, 12, 31)
            };

            var attr = new DateEarlierThanAttribute("EndDate");
            var result = attr.GetValidationResult(model.StartDate, GetContext(model, nameof(model.StartDate)));

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void Invalid_When_StartDateIsNotEarlier()
        {
            var model = new SampleModel
            {
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2023, 12, 31)
            };

            var attr = new DateEarlierThanAttribute("EndDate") { ErrorMessage = "StartDate must be earlier than EndDate" };
            var result = attr.GetValidationResult(model.StartDate, GetContext(model, nameof(model.StartDate)));

            Assert.NotNull(result);
            Assert.Equal("StartDate must be earlier than EndDate", result.ErrorMessage);
        }

        [Fact]
        public void Valid_When_ValueIsNull()
        {
            var model = new SampleModel
            {
                EndDate = new DateTime(2023, 12, 31)
            };

            var attr = new DateEarlierThanAttribute("EndDate");
            var result = attr.GetValidationResult(null, GetContext(model, nameof(model.StartDate)));

            Assert.Equal(ValidationResult.Success, result);
        }

        // Test class to simulate the object being validated
        public class TestModel
        {
            [DateEarlierThan("EndDate")]
            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            [DateEarlierThan("DeliveryDate", ErrorMessage = "Order date must be before delivery date")]
            public DateTime OrderDate { get; set; }

            public DateTime DeliveryDate { get; set; }

            [DateEarlierThan("CompletionDate")]
            public DateTime? OptionalStartDate { get; set; }

            public DateTime? CompletionDate { get; set; }

            [DateEarlierThan("NonExistentProperty")]
            public DateTime TestDate { get; set; }

            // Non-DateTime property for type validation testing
            public string StringProperty { get; set; }

            [DateEarlierThan("StringProperty")]
            public DateTime DateWithStringComparison { get; set; }
        }

        [Fact]
        public void IsValid_WhenThisDateIsEarlier_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.StartDate),
                DisplayName = "Start Date"
            };
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Act
            var result = attribute.GetValidationResult(model.StartDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenThisDateIsLater_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateTime(2023, 12, 31),
                EndDate = new DateTime(2023, 1, 1)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.StartDate),
                DisplayName = "Start Date"
            };
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Act
            var result = attribute.GetValidationResult(model.StartDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Start Date must be earlier than EndDate", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenThisDateIsEqual_ReturnsValidationError()
        {
            // Arrange
            var sameDate = new DateTime(2023, 6, 15);
            var model = new TestModel
            {
                StartDate = sameDate,
                EndDate = sameDate
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.StartDate),
                DisplayName = "Start Date"
            };
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Act
            var result = attribute.GetValidationResult(model.StartDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Start Date must be earlier than EndDate", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var model = new TestModel
            {
                OrderDate = new DateTime(2023, 12, 31),
                DeliveryDate = new DateTime(2023, 1, 1)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OrderDate),
                DisplayName = "Order Date"
            };
            var attribute = new DateEarlierThanAttribute("DeliveryDate")
            {
                ErrorMessage = "Order date must be before delivery date"
            };

            // Act
            var result = attribute.GetValidationResult(model.OrderDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Order date must be before delivery date", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenThisValueIsNull_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                OptionalStartDate = null,
                CompletionDate = new DateTime(2023, 12, 31)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalStartDate),
                DisplayName = "Optional Start Date"
            };
            var attribute = new DateEarlierThanAttribute("CompletionDate");

            // Act
            var result = attribute.GetValidationResult(model.OptionalStartDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenOtherValueIsNull_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                OptionalStartDate = new DateTime(2023, 1, 1),
                CompletionDate = null
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalStartDate),
                DisplayName = "Optional Start Date"
            };
            var attribute = new DateEarlierThanAttribute("CompletionDate");

            // Act
            var result = attribute.GetValidationResult(model.OptionalStartDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenBothValuesAreNull_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                OptionalStartDate = null,
                CompletionDate = null
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalStartDate),
                DisplayName = "Optional Start Date"
            };
            var attribute = new DateEarlierThanAttribute("CompletionDate");

            // Act
            var result = attribute.GetValidationResult(model.OptionalStartDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenOtherPropertyDoesNotExist_ReturnsPropertyNotFoundError()
        {
            // Arrange
            var model = new TestModel
            {
                TestDate = new DateTime(2023, 1, 1)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.TestDate),
                DisplayName = "Test Date"
            };
            var attribute = new DateEarlierThanAttribute("NonExistentProperty");

            // Act
            var result = attribute.GetValidationResult(model.TestDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Unknown property: NonExistentProperty", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenThisValueIsNotDateTime_ReturnsTypeError()
        {
            // Arrange
            var model = new TestModel
            {
                EndDate = new DateTime(2023, 12, 31)
            };

            var context = new ValidationContext(model)
            {
                MemberName = "SomeProperty",
                DisplayName = "Some Property"
            };
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Act
            var result = attribute.GetValidationResult("not a date", context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Some Property and EndDate must be DateTime types", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenOtherValueIsNotDateTime_ReturnsTypeError()
        {
            // Arrange
            var model = new TestModel
            {
                DateWithStringComparison = new DateTime(2023, 1, 1),
                StringProperty = "not a date"
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.DateWithStringComparison),
                DisplayName = "Date With String Comparison"
            };
            var attribute = new DateEarlierThanAttribute("StringProperty");

            // Act
            var result = attribute.GetValidationResult(model.DateWithStringComparison, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Date With String Comparison and StringProperty must be DateTime types", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNullableDateTime_ValidComparison_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                OptionalStartDate = new DateTime(2023, 1, 1),
                CompletionDate = new DateTime(2023, 12, 31)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalStartDate),
                DisplayName = "Optional Start Date"
            };
            var attribute = new DateEarlierThanAttribute("CompletionDate");

            // Act
            var result = attribute.GetValidationResult(model.OptionalStartDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNullableDateTime_InvalidComparison_ReturnsError()
        {
            // Arrange
            var model = new TestModel
            {
                OptionalStartDate = new DateTime(2023, 12, 31),
                CompletionDate = new DateTime(2023, 1, 1)
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.OptionalStartDate),
                DisplayName = "Optional Start Date"
            };
            var attribute = new DateEarlierThanAttribute("CompletionDate");

            // Act
            var result = attribute.GetValidationResult(model.OptionalStartDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Optional Start Date must be earlier than CompletionDate", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithTimeComponent_EarlierTime_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateTime(2023, 6, 15, 10, 30, 0), // 10:30 AM
                EndDate = new DateTime(2023, 6, 15, 14, 45, 0)    // 2:45 PM same day
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.StartDate),
                DisplayName = "Start Date"
            };
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Act
            var result = attribute.GetValidationResult(model.StartDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithTimeComponent_LaterTime_ReturnsError()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateTime(2023, 6, 15, 14, 45, 0), // 2:45 PM
                EndDate = new DateTime(2023, 6, 15, 10, 30, 0)    // 10:30 AM same day
            };

            var context = new ValidationContext(model)
            {
                MemberName = nameof(TestModel.StartDate),
                DisplayName = "Start Date"
            };
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Act
            var result = attribute.GetValidationResult(model.StartDate, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Start Date must be earlier than EndDate", result.ErrorMessage);
        }

        [Fact]
        public void Constructor_WithValidPropertyName_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new DateEarlierThanAttribute("EndDate");

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void Constructor_WithNullPropertyName_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new DateEarlierThanAttribute(null);

            // Assert
            Assert.NotNull(attribute);
        }

    }
}

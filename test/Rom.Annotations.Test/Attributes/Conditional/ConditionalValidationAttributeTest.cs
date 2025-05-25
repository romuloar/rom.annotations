using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class ConditionalValidationAttributeTest
    {
        // Test class to simulate the object being validated
        public class TestModel
        {
            public string Status { get; set; }
            public int Priority { get; set; }
            public bool IsActive { get; set; }

            [ConditionalValidation("Status", "Active", typeof(RequiredAttribute))]
            public string RequiredWhenActive { get; set; }

            [ConditionalValidation("Status", "Active", typeof(StringLengthAttribute), 10)]
            public string MaxLengthWhenActive { get; set; }

            [ConditionalValidation("IsActive", true, typeof(RangeAttribute), 1, 10)]
            public int RangeWhenTrue { get; set; }

            [ConditionalValidation("Priority", 1, typeof(RequiredAttribute))]
            public string RequiredWhenPriorityOne { get; set; }
        }

        [Fact]
        public void IsValid_WhenConditionNotMet_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                Status = "Inactive",
                RequiredWhenActive = null // Would be invalid if Status was "Active"
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredWhenActive) };
            var attribute = new ConditionalValidationAttribute("Status", "Active", typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult(model.RequiredWhenActive, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenConditionMetAndValueValid_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                Status = "Active",
                RequiredWhenActive = "Valid Value"
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredWhenActive) };
            var attribute = new ConditionalValidationAttribute("Status", "Active", typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult(model.RequiredWhenActive, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenConditionMetAndValueInvalid_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel
            {
                Status = "Active",
                RequiredWhenActive = null // Invalid when Status is "Active"
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredWhenActive) };
            var attribute = new ConditionalValidationAttribute("Status", "Active", typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult(model.RequiredWhenActive, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Contains("required", result.ErrorMessage.ToLower());
        }

        [Fact]
        public void IsValid_WithStringLengthValidation_WhenExceedsLength_ReturnsError()
        {
            // Arrange
            var model = new TestModel
            {
                Status = "Active",
                MaxLengthWhenActive = "This string is longer than 10 characters"
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.MaxLengthWhenActive) };
            var attribute = new ConditionalValidationAttribute("Status", "Active", typeof(StringLengthAttribute), 10);

            // Act
            var result = attribute.GetValidationResult(model.MaxLengthWhenActive, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithStringLengthValidation_WhenWithinLength_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                Status = "Active",
                MaxLengthWhenActive = "Short"
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.MaxLengthWhenActive) };
            var attribute = new ConditionalValidationAttribute("Status", "Active", typeof(StringLengthAttribute), 10);

            // Act
            var result = attribute.GetValidationResult(model.MaxLengthWhenActive, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithBooleanCondition_WhenTrue_ValidatesRange()
        {
            // Arrange
            var model = new TestModel
            {
                IsActive = true,
                RangeWhenTrue = 15 // Outside range 1-10
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RangeWhenTrue) };
            var attribute = new ConditionalValidationAttribute("IsActive", true, typeof(RangeAttribute), 1, 10);

            // Act
            var result = attribute.GetValidationResult(model.RangeWhenTrue, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithBooleanCondition_WhenFalse_IgnoresValidation()
        {
            // Arrange
            var model = new TestModel
            {
                IsActive = false,
                RangeWhenTrue = 15 // Outside range, but condition is not met
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RangeWhenTrue) };
            var attribute = new ConditionalValidationAttribute("IsActive", true, typeof(RangeAttribute), 1, 10);

            // Act
            var result = attribute.GetValidationResult(model.RangeWhenTrue, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNumericCondition_WhenMatches_ValidatesRequired()
        {
            // Arrange
            var model = new TestModel
            {
                Priority = 1,
                RequiredWhenPriorityOne = null // Invalid when Priority = 1
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredWhenPriorityOne) };
            var attribute = new ConditionalValidationAttribute("Priority", 1, typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult(model.RequiredWhenPriorityOne, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNumericCondition_WhenDoesNotMatch_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                Priority = 2,
                RequiredWhenPriorityOne = null // Valid when Priority != 1
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredWhenPriorityOne) };
            var attribute = new ConditionalValidationAttribute("Priority", 1, typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult(model.RequiredWhenPriorityOne, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenConditionFieldDoesNotExist_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel();
            var context = new ValidationContext(model) { MemberName = "SomeProperty" };
            var attribute = new ConditionalValidationAttribute("NonExistentField", "SomeValue", typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult("SomeValue", context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNullExpectedValue_WhenConditionFieldIsNull_ValidatesInnerAttribute()
        {
            // Arrange
            var model = new TestModel
            {
                Status = null,
                RequiredWhenActive = null
            };

            var context = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredWhenActive) };
            var attribute = new ConditionalValidationAttribute("Status", null, typeof(RequiredAttribute));

            // Act
            var result = attribute.GetValidationResult(model.RequiredWhenActive, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new ConditionalValidationAttribute("TestField", "TestValue", typeof(RequiredAttribute));

            // Assert
            Assert.NotNull(attribute);
        }
    }
}

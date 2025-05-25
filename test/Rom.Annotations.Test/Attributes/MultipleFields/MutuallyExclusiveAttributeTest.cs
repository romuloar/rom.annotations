using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations.Test.Attributes.MultipleFields
{
    public class MutuallyExclusiveAttributeTest
    {
        private class Model
        {
            [MutuallyExclusive("FieldB", "FieldC")]
            public string FieldA { get; set; }
            public string FieldB { get; set; }
            public string FieldC { get; set; }
        }

        [Fact]
        public void Valid_When_OnlyOneFieldIsFilled()
        {
            var model = new Model { FieldB = "Hello" };
            var ctx = new ValidationContext(model);
            var result = Validator.TryValidateObject(model, ctx, null, true);
            Assert.True(result);
        }
        
        private class TestModel
        {
            public string Field1 { get; set; }
            public string Field2 { get; set; }
            public string Field3 { get; set; }
            public int? IntField1 { get; set; }
            public int? IntField2 { get; set; }
        }

        [Fact]
        public void IsValid_WhenNoFieldsAreFilled_ShouldReturnSuccess()
        {
            // Arrange
            var model = new TestModel();
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2", "Field3");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenOnlyOneFieldIsFilled_ShouldReturnSuccess()
        {
            // Arrange
            var model = new TestModel { Field1 = "value" };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2", "Field3");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenTwoFieldsAreFilled_ShouldReturnValidationError()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "value1",
                Field2 = "value2"
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2", "Field3");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Fields are mutually exclusive.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenAllFieldsAreFilled_ShouldReturnValidationError()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "value1",
                Field2 = "value2",
                Field3 = "value3"
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2", "Field3");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Fields are mutually exclusive.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithCustomErrorMessage_ShouldReturnCustomMessage()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "value1",
                Field2 = "value2"
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2")
            {
                ErrorMessage = "Custom error message"
            };
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Custom error message", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WhenStringFieldIsEmpty_ShouldTreatAsNotFilled()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "",
                Field2 = "value2"
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenStringFieldIsWhitespace_ShouldTreatAsNotFilled()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "   ",
                Field2 = "value2"
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WhenStringFieldIsNull_ShouldTreatAsNotFilled()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = null,
                Field2 = "value2"
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "Field2");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNullableIntFields_WhenOnlyOneHasValue_ShouldReturnSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                IntField1 = 123,
                IntField2 = null
            };
            var attribute = new MutuallyExclusiveAttribute("IntField1", "IntField2");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithNullableIntFields_WhenBothHaveValues_ShouldReturnValidationError()
        {
            // Arrange
            var model = new TestModel
            {
                IntField1 = 123,
                IntField2 = 456
            };
            var attribute = new MutuallyExclusiveAttribute("IntField1", "IntField2");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Fields are mutually exclusive.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNonExistentProperty_ShouldNotThrowException()
        {
            // Arrange
            var model = new TestModel { Field1 = "value" };
            var attribute = new MutuallyExclusiveAttribute("Field1", "NonExistentField");
            var context = new ValidationContext(model);

            // Act & Assert
            var exception = Record.Exception(() => attribute.GetValidationResult(null, context));
            Assert.Null(exception);
        }

        [Fact]
        public void IsValid_WithSingleProperty_WhenFilled_ShouldReturnSuccess()
        {
            // Arrange
            var model = new TestModel { Field1 = "value" };
            var attribute = new MutuallyExclusiveAttribute("Field1");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithMixedFieldTypes_WhenOnlyOneIsFilled_ShouldReturnSuccess()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "value",
                IntField1 = null
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "IntField1");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithMixedFieldTypes_WhenBothAreFilled_ShouldReturnValidationError()
        {
            // Arrange
            var model = new TestModel
            {
                Field1 = "value",
                IntField1 = 123
            };
            var attribute = new MutuallyExclusiveAttribute("Field1", "IntField1");
            var context = new ValidationContext(model);

            // Act
            var result = attribute.GetValidationResult(null, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Fields are mutually exclusive.", result.ErrorMessage);
        }
    }
}

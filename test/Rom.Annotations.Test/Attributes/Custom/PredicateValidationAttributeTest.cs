using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Custom
{
    public class PredicateValidationAttributeTest
    {
        // Test class to simulate the object being validated
        public class TestModel
        {
            [PredicateValidation("IsValidEmail")]
            public string Email { get; set; }

            [PredicateValidation("IsValidAge", ErrorMessage = "Age must be between 18 and 65")]
            public int Age { get; set; }

            [PredicateValidation("IsValidPassword")]
            public string Password { get; set; }

            [PredicateValidation("IsValidPrice")]
            public decimal Price { get; set; }

            // Public validation method
            public bool IsValidEmail(object value)
            {
                if (value is string email)
                {
                    return !string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains(".");
                }
                return false;
            }

            // Private validation method
            private bool IsValidAge(object value)
            {
                if (value is int age)
                {
                    return age >= 18 && age <= 65;
                }
                return false;
            }

            // Method that returns true
            public bool IsValidPassword(object value)
            {
                if (value is string password)
                {
                    return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
                }
                return false;
            }

            // Method that validates decimal values
            public bool IsValidPrice(object value)
            {
                if (value is decimal price)
                {
                    return price > 0 && price <= 10000;
                }
                return false;
            }
        }

        // Test class without validation methods
        public class InvalidTestModel
        {
            [PredicateValidation("NonExistentMethod")]
            public string TestProperty { get; set; }
        }

        // Test class with method that returns non-boolean
        public class NonBooleanMethodModel
        {
            [PredicateValidation("ReturnString")]
            public string TestProperty { get; set; }

            public string ReturnString(object value)
            {
                return "not a boolean";
            }
        }

        [Fact]
        public void IsValid_WithValidEmail_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { Email = "test@example.com" };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
            var attribute = new PredicateValidationAttribute("IsValidEmail");

            // Act
            var result = attribute.GetValidationResult(model.Email, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithInvalidEmail_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel { Email = "invalid-email" };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
            var attribute = new PredicateValidationAttribute("IsValidEmail");

            // Act
            var result = attribute.GetValidationResult(model.Email, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var model = new TestModel { Age = 17 }; // Invalid age
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Age) };
            var attribute = new PredicateValidationAttribute("IsValidAge")
            {
                ErrorMessage = "Age must be between 18 and 65"
            };

            // Act
            var result = attribute.GetValidationResult(model.Age, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Age must be between 18 and 65", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithValidAge_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { Age = 25 };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Age) };
            var attribute = new PredicateValidationAttribute("IsValidAge");

            // Act
            var result = attribute.GetValidationResult(model.Age, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithInvalidAge_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel { Age = 70 }; // Too old
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Age) };
            var attribute = new PredicateValidationAttribute("IsValidAge");

            // Act
            var result = attribute.GetValidationResult(model.Age, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithValidPassword_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { Password = "SecurePassword123" };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Password) };
            var attribute = new PredicateValidationAttribute("IsValidPassword");

            // Act
            var result = attribute.GetValidationResult(model.Password, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithInvalidPassword_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel { Password = "short" }; // Too short
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Password) };
            var attribute = new PredicateValidationAttribute("IsValidPassword");

            // Act
            var result = attribute.GetValidationResult(model.Password, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithValidPrice_ReturnsSuccess()
        {
            // Arrange
            var model = new TestModel { Price = 99.99m };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Price) };
            var attribute = new PredicateValidationAttribute("IsValidPrice");

            // Act
            var result = attribute.GetValidationResult(model.Price, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_WithInvalidPrice_ReturnsValidationError()
        {
            // Arrange
            var model = new TestModel { Price = -10.00m }; // Negative price
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Price) };
            var attribute = new PredicateValidationAttribute("IsValidPrice");

            // Act
            var result = attribute.GetValidationResult(model.Price, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNonExistentMethod_ReturnsMethodNotFoundError()
        {
            // Arrange
            var model = new InvalidTestModel { TestProperty = "test" };
            var context = new ValidationContext(model) { MemberName = nameof(InvalidTestModel.TestProperty) };
            var attribute = new PredicateValidationAttribute("NonExistentMethod");

            // Act
            var result = attribute.GetValidationResult(model.TestProperty, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Method 'NonExistentMethod' not found.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNonBooleanReturnType_ReturnsValidationError()
        {
            // Arrange
            var model = new NonBooleanMethodModel { TestProperty = "test" };
            var context = new ValidationContext(model) { MemberName = nameof(NonBooleanMethodModel.TestProperty) };
            var attribute = new PredicateValidationAttribute("ReturnString");

            // Act
            var result = attribute.GetValidationResult(model.TestProperty, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithNullValue_HandledByPredicateMethod()
        {
            // Arrange
            var model = new TestModel { Email = null };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
            var attribute = new PredicateValidationAttribute("IsValidEmail");

            // Act
            var result = attribute.GetValidationResult(model.Email, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_WithEmptyString_HandledByPredicateMethod()
        {
            // Arrange
            var model = new TestModel { Email = "" };
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
            var attribute = new PredicateValidationAttribute("IsValidEmail");

            // Act
            var result = attribute.GetValidationResult(model.Email, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Predicate validation failed.", result.ErrorMessage);
        }

        [Fact]
        public void Constructor_WithValidMethodName_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new PredicateValidationAttribute("TestMethod");

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void Constructor_WithNullMethodName_CreatesAttributeSuccessfully()
        {
            // Arrange & Act
            var attribute = new PredicateValidationAttribute(null);

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void IsValid_AccessesPrivateMethod_ReturnsCorrectResult()
        {
            // Arrange
            var model = new TestModel { Age = 30 }; // Valid age for private method
            var context = new ValidationContext(model) { MemberName = nameof(TestModel.Age) };
            var attribute = new PredicateValidationAttribute("IsValidAge");

            // Act
            var result = attribute.GetValidationResult(model.Age, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }
    }
}

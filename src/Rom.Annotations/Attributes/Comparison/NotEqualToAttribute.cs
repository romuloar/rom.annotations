using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the value of the current field is NOT equal to the value of the specified other field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotEqualToAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotEqualToAttribute"/> class.
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property to compare with.</param>
        public NotEqualToAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            // If both null or both equal, validation fails
            if (Equals(value, otherValue))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must not be equal to {_otherPropertyName}.");
            }

            // Valid if values are different
            return ValidationResult.Success;
        }
    }
}

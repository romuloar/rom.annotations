using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the value of the date property is later than the value of another date property.
    /// Supports DateTime and nullable DateTime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateLaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        /// <summary>
        /// Constructor specifying the other property to compare with.
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property.</param>
        public DateLaterThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the other property
            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
            {
                return new ValidationResult($"Unknown property: {_otherPropertyName}");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            // Nulls are considered valid (use [Required] to enforce presence)
            if (value == null || otherValue == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime thisDate && otherValue is DateTime otherDate)
            {
                if (thisDate > otherDate)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }

            return new ValidationResult("Both properties must be of type DateTime.");
        }
    }
}

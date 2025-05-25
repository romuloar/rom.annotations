using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a string property is not null, empty or whitespace.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredStringAttribute : ValidationAttribute
    {
        /// <summary>
        /// Checks if the string is not null, empty or whitespace.
        /// </summary>
        /// <param name="value">The value of the property to validate.</param>
        /// <returns>ValidationResult.Success if valid; otherwise, an error message.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                // Use the ErrorMessage or default message
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a Guid property is not Guid.Empty.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredGuidAttribute : ValidationAttribute
    {
        /// <summary>
        /// Checks if the Guid is not Guid.Empty.
        /// </summary>
        /// <param name="value">The value of the property to validate.</param>
        /// <param name="validationContext">Context information.</param>
        /// <returns>ValidationResult.Success if valid; otherwise, an error message.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            if (value is Guid guidValue)
            {
                if (guidValue == Guid.Empty)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                return ValidationResult.Success;
            }

            // Not a Guid type, so invalid usage
            return new ValidationResult($"{validationContext.DisplayName} must be a Guid.");
        }
    }
}

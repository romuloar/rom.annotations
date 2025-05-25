using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations.Attributes.String
{
    /// <summary>
    /// Validates that the string length is exactly equal to the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StringLengthEqualsAttribute : ValidationAttribute
    {
        /// <summary>
        /// The required string length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringLengthEqualsAttribute"/> class.
        /// </summary>
        /// <param name="length">The exact required length of the string.</param>
        public StringLengthEqualsAttribute(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
            Length = length;
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"/> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Null values are considered valid. Use [Required] for null checks.
            if (value == null)
                return ValidationResult.Success;

            var str = value as string;
            if (str == null)
                return new ValidationResult("The value is not a string.");

            if (str.Length == Length)
                return ValidationResult.Success;

            var errorMessage = ErrorMessage ?? $"The field {validationContext.DisplayName} must be exactly {Length} characters long.";
            return new ValidationResult(errorMessage);
        }
    }
}

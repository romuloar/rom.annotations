using System;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a DateTime property is within a specified inclusive range.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateTime MinDate { get; }
        public DateTime MaxDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeAttribute"/> class.
        /// </summary>
        /// <param name="minDate">The minimum allowed date (inclusive).</param>
        /// <param name="maxDate">The maximum allowed date (inclusive).</param>
        public DateRangeAttribute(string minDate, string maxDate)
        {
            if (!DateTime.TryParse(minDate, out var min))
                throw new ArgumentException("Invalid minDate format", nameof(minDate));
            if (!DateTime.TryParse(maxDate, out var max))
                throw new ArgumentException("Invalid maxDate format", nameof(maxDate));
            if (min > max)
                throw new ArgumentException("minDate must be less than or equal to maxDate");

            MinDate = min;
            MaxDate = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Nulls allowed, use [Required] if needed

            if (value is DateTime dateValue)
            {
                if (dateValue < MinDate || dateValue > MaxDate)
                {
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName ?? "Date"));
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("The field must be a valid DateTime.");
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? $"{name} must be between {MinDate:yyyy-MM-dd} and {MaxDate:yyyy-MM-dd} (inclusive).";
        }
    }
}

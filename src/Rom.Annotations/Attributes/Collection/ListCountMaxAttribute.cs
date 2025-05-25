using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a collection has at most a maximum number of items.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListCountMaxAttribute : ValidationAttribute
    {
        public int MaxCount { get; }

        public ListCountMaxAttribute(int maxCount)
        {
            if (maxCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount), "MaxCount cannot be negative");

            MaxCount = maxCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Allow null, use [Required] if necessary

            if (value is ICollection collection)
            {
                if (collection.Count > MaxCount)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must contain at most {MaxCount} item(s).");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid collection.");
        }
    }
}

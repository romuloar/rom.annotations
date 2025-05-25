using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a collection has at least a minimum number of items.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListCountMinAttribute : ValidationAttribute
    {
        public int MinCount { get; }

        public ListCountMinAttribute(int minCount)
        {
            if (minCount < 0)
                throw new ArgumentOutOfRangeException(nameof(minCount), "MinCount cannot be negative");

            MinCount = minCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Allow null, use [Required] if necessary

            if (value is ICollection collection)
            {
                if (collection.Count < MinCount)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must contain at least {MinCount} item(s).");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid collection.");
        }
    }
}

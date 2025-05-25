using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a collection has between a minimum and maximum number of items.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListCountRangeAttribute : ValidationAttribute
    {
        public int MinCount { get; }
        public int MaxCount { get; }

        public ListCountRangeAttribute(int minCount, int maxCount)
        {
            if (minCount < 0)
                throw new ArgumentOutOfRangeException(nameof(minCount), "MinCount cannot be negative");
            if (maxCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount), "MaxCount cannot be negative");
            if (minCount > maxCount)
                throw new ArgumentException("MinCount cannot be greater than MaxCount");

            MinCount = minCount;
            MaxCount = maxCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Allow null, use [Required] if necessary

            if (value is ICollection collection)
            {
                if (collection.Count < MinCount || collection.Count > MaxCount)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must contain between {MinCount} and {MaxCount} items.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid collection.");
        }
    }
}

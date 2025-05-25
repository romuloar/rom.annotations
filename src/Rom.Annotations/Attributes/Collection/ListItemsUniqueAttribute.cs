using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that all items in a collection are unique.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListItemsUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Allow null, use [Required] if necessary

            if (value is IEnumerable enumerable)
            {
                var set = new HashSet<object>();
                foreach (var item in enumerable)
                {
                    if (item == null)
                        continue;

                    if (!set.Add(item))
                    {
                        return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} contains duplicate items.");
                    }
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid collection.");
        }
    }
}

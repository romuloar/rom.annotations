using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a collection property is not null and contains at least one item.
    /// Supports IEnumerable, arrays, List, Collection, etc.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (value is IEnumerable enumerable)
            {
                foreach (var _ in enumerable)
                {
                    // Found at least one item
                    return true;
                }
                // No items found
                return false;
            }

            // Not a collection
            return false;
        }
    }
}

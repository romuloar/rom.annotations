using System;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a DateTime or nullable DateTime property is in UTC.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateIsUtcAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates that the value is a UTC DateTime.
        /// </summary>
        /// <param name="value">The value of the property to validate.</param>
        /// <returns>True if the value is a UTC DateTime or null (nullable), false otherwise.</returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return true; // Consider null as valid, use [Required] if needed

            if (value is DateTime dt)
            {
                return dt.Kind == DateTimeKind.Utc;
            }

            return false; // Invalid if not a DateTime
        }
    }
}

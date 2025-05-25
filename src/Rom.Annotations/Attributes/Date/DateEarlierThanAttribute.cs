using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the date value is earlier than the date of another property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateEarlierThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public DateEarlierThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Null is valid

            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            object otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (otherValue == null)
                return ValidationResult.Success; // if other property null, valid

            if (!(value is DateTime thisDate) || !(otherValue is DateTime otherDate))
            {
                return new ValidationResult($"{validationContext.DisplayName} and {_otherPropertyName} must be DateTime types");
            }

            if (thisDate < otherDate)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be earlier than {_otherPropertyName}");
        }
    }
}

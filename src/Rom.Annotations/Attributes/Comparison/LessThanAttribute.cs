using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the value of the property is less than the value of another property.
    /// Supports numeric types and DateTime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LessThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public LessThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Null values are valid here

            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);

            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            object otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (otherValue == null)
                return ValidationResult.Success; // if other property is null, validation passes

            try
            {
                // Convert both values to decimal for numeric comparison if possible
                if (IsNumber(value) && IsNumber(otherValue))
                {
                    decimal decValue = Convert.ToDecimal(value);
                    decimal decOtherValue = Convert.ToDecimal(otherValue);

                    if (decValue < decOtherValue)
                        return ValidationResult.Success;
                }
                else if (value is DateTime valDate && otherValue is DateTime otherDate)
                {
                    if (valDate < otherDate)
                        return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"LessThanAttribute only supports numeric and DateTime types");
                }
            }
            catch
            {
                return new ValidationResult($"Error comparing properties: {validationContext.MemberName} and {_otherPropertyName}");
            }

            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be less than {_otherPropertyName}");
        }

        private bool IsNumber(object value)
        {
            return value is byte || value is sbyte
                || value is short || value is ushort
                || value is int || value is uint
                || value is long || value is ulong
                || value is float || value is double
                || value is decimal;
        }
    }
}

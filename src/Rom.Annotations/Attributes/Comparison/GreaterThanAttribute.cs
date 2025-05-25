using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the current field is greater than the value of another specified field.
    /// Works for numeric types and DateTime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public GreaterThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);
            if (value == null || otherValue == null)
                return ValidationResult.Success; // consider nulls valid

            try
            {
                // Convert both values to IComparable for comparison
                IComparable compValue = value as IComparable;
                IComparable compOther = otherValue as IComparable;

                if (compValue == null || compOther == null)
                    return new ValidationResult("Both properties must be comparable.");

                if (compValue.CompareTo(compOther) <= 0)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be greater than {_otherPropertyName}.");
                }
            }
            catch
            {
                return new ValidationResult("Error comparing properties.");
            }

            return ValidationResult.Success;
        }
    }
}

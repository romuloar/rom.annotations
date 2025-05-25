using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validation attribute that makes a field required if another boolean field is false.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfFalseAttribute : ValidationAttribute
    {
        private readonly string _booleanPropertyName;

        /// <summary>
        /// Creates a new instance of <see cref="RequiredIfFalseAttribute"/>.
        /// </summary>
        /// <param name="booleanPropertyName">The name of the boolean property to check.</param>
        public RequiredIfFalseAttribute(string booleanPropertyName)
        {
            _booleanPropertyName = booleanPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the boolean property info
            PropertyInfo booleanProperty = validationContext.ObjectType.GetProperty(_booleanPropertyName);

            if (booleanProperty == null)
            {
                return new ValidationResult($"Unknown property: {_booleanPropertyName}");
            }

            // Get the value of the boolean property
            object booleanValueObj = booleanProperty.GetValue(validationContext.ObjectInstance);

            if (booleanValueObj == null)
            {
                return ValidationResult.Success; // If the boolean property is null, do not validate
            }

            if (!(booleanValueObj is bool booleanValue))
            {
                return new ValidationResult($"{_booleanPropertyName} is not a boolean property.");
            }

            // If boolean property is false, the current field must be not null or not empty (for strings)
            if (booleanValue == false)
            {
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} is required because {_booleanPropertyName} is false.");
                }

                // If value is string, check if empty or whitespace
                if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} is required because {_booleanPropertyName} is false.");
                }
            }

            // Otherwise valid
            return ValidationResult.Success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Makes the field required if another property is not null.
    /// Useful when a field must be filled only when another is present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfNotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        public string DependentProperty { get; }

        public RequiredIfNotNullOrWhiteSpaceAttribute(string dependentProperty)
        {
            DependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentPropertyInfo = validationContext.ObjectType.GetProperty(DependentProperty);
            if (dependentPropertyInfo == null)
                return new ValidationResult($"Property '{DependentProperty}' not found.");

            var dependentValue = dependentPropertyInfo.GetValue(validationContext.ObjectInstance);

            // If dependent property is NOT null, then this field becomes required
            if (dependentValue != null)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {DependentProperty} is not null.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

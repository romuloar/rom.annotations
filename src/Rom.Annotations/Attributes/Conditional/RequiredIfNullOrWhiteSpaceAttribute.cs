using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Makes the field required if another property is null or an empty string.
    /// Supports string and nullable types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        public string DependentProperty { get; }

        public RequiredIfNullOrWhiteSpaceAttribute(string dependentProperty)
        {
            DependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentPropertyInfo = validationContext.ObjectType.GetProperty(DependentProperty);

            if (dependentPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {DependentProperty}");
            }

            var dependentValue = dependentPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var isNullOrEmpty = dependentValue == null ||
                                (dependentValue is string str && string.IsNullOrWhiteSpace(str));

            if (isNullOrEmpty)
            {
                var isValid = value != null && (!(value is string s) || !string.IsNullOrWhiteSpace(s));

                if (!isValid)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required because {DependentProperty} is null or empty.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

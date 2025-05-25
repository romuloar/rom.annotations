using System;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Makes the field required if another boolean property is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfTrueAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;

        /// <param name="dependentProperty">Name of the boolean property that must be true for this field to be required.</param>
        public RequiredIfTrueAttribute(string dependentProperty)
        {
            _dependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(_dependentProperty);

            if (field == null)
                return new ValidationResult($"Property '{_dependentProperty}' not found.");

            var dependentValue = field.GetValue(validationContext.ObjectInstance);

            if (dependentValue is bool boolValue && boolValue)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {_dependentProperty} is true.");
            }

            return ValidationResult.Success;
        }
    }
}

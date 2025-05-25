using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Makes the field required if the value of another field is in the specified set of values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfInSetAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;
        private readonly object[] _targetValues;

        /// <summary>
        /// Constructor that sets the dependent property and the set of values to check.
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property to check.</param>
        /// <param name="targetValues">The set of values that make this field required.</param>
        public RequiredIfInSetAttribute(string otherPropertyName, params object[] targetValues)
        {
            _otherPropertyName = otherPropertyName ?? throw new ArgumentNullException(nameof(otherPropertyName));
            _targetValues = targetValues ?? throw new ArgumentNullException(nameof(targetValues));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the property info of the other property
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            // Get the value of the other property
            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

            // Check if the other property's value is in the target values set
            bool isInSet = _targetValues.Any(target => Equals(target, otherValue));

            if (isInSet)
            {
                // When in set, this field is required (not null or empty if string)
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
                }
                else if (value is string s && string.IsNullOrWhiteSpace(s))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

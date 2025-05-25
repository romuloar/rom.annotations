using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates if a numeric field is within a given range only if another field has a specific value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RangeIfAttribute : ValidationAttribute
    {
        private readonly string _dependentPropertyName;
        private readonly object _dependentValue;
        private readonly double _minimum;
        private readonly double _maximum;

        /// <summary>
        /// Creates a new instance of <see cref="RangeIfAttribute"/>.
        /// </summary>
        /// <param name="dependentPropertyName">The name of the property to check the value.</param>
        /// <param name="dependentValue">The value to match in the dependent property for the range validation to apply.</param>
        /// <param name="minimum">The minimum value allowed.</param>
        /// <param name="maximum">The maximum value allowed.</param>
        public RangeIfAttribute(string dependentPropertyName, object dependentValue, double minimum, double maximum)
        {
            _dependentPropertyName = dependentPropertyName;
            _dependentValue = dependentValue;
            _minimum = minimum;
            _maximum = maximum;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get dependent property info
            PropertyInfo dependentProperty = validationContext.ObjectType.GetProperty(_dependentPropertyName);

            if (dependentProperty == null)
            {
                return new ValidationResult($"Unknown property: {_dependentPropertyName}");
            }

            // Get the dependent property value
            object dependentPropertyValue = dependentProperty.GetValue(validationContext.ObjectInstance);

            // Check if dependent property value equals the target dependentValue (considering null)
            if ((dependentPropertyValue == null && _dependentValue == null) ||
                (dependentPropertyValue != null && dependentPropertyValue.Equals(_dependentValue)))
            {
                // If this property is null or not a number, skip range validation
                if (value == null)
                {
                    return ValidationResult.Success; // no value to validate range
                }

                double doubleValue;

                try
                {
                    doubleValue = Convert.ToDouble(value);
                }
                catch (Exception)
                {
                    return new ValidationResult($"{validationContext.MemberName} is not a numeric type.");
                }

                if (doubleValue < _minimum || doubleValue > _maximum)
                {
                    return new ValidationResult(ErrorMessage ??
                        $"{validationContext.MemberName} must be between {_minimum} and {_maximum} when {_dependentPropertyName} is {_dependentValue}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

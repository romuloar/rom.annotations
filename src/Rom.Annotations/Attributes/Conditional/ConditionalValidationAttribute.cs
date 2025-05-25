using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validation attribute that conditionally applies another validation attribute based on the value of a specified field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalValidationAttribute : ValidationAttribute
    {
        private readonly string _conditionField;
        private readonly object _expectedValue;
        private readonly ValidationAttribute _innerAttribute;

        public ConditionalValidationAttribute(string conditionField, object expectedValue, Type validatorType, params object[] validatorArgs)
        {
            _conditionField = conditionField;
            _expectedValue = expectedValue;
            _innerAttribute = (ValidationAttribute)Activator.CreateInstance(validatorType, validatorArgs);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conditionProperty = validationContext.ObjectType.GetProperty(_conditionField);
            if (conditionProperty == null)
                return ValidationResult.Success;

            var conditionValue = conditionProperty.GetValue(validationContext.ObjectInstance);

            if (Equals(conditionValue, _expectedValue))
            {
                var result = _innerAttribute.GetValidationResult(value, validationContext);
                if (result != ValidationResult.Success)
                    return result;
            }

            return ValidationResult.Success;
        }
    }
}

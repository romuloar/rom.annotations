using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates each item in a collection using a specified ValidationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListItemsConditionAttribute : ValidationAttribute
    {
        private readonly ValidationAttribute _itemValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListItemsConditionAttribute"/> class.
        /// </summary>
        /// <param name="itemValidatorType">The type of the validation attribute to apply to each item (must inherit from ValidationAttribute).</param>
        /// <param name="constructorArgs">Constructor arguments for the item validation attribute.</param>
        public ListItemsConditionAttribute(Type itemValidatorType, params object[] constructorArgs)
        {
            if (itemValidatorType == null)
                throw new ArgumentNullException(nameof(itemValidatorType));
            if (!typeof(ValidationAttribute).IsAssignableFrom(itemValidatorType))
                throw new ArgumentException("itemValidatorType must be a ValidationAttribute type.");

            try
            {
                _itemValidator = (ValidationAttribute)Activator.CreateInstance(itemValidatorType, constructorArgs);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create an instance of the validator attribute.", ex);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // allow null

            if (value is IEnumerable enumerable)
            {
                int index = 0;
                foreach (var item in enumerable)
                {
                    var result = _itemValidator.GetValidationResult(item, new ValidationContext(item ?? new object()));
                    if (result != ValidationResult.Success)
                    {
                        var errorMessage = ErrorMessage ?? $"{validationContext.DisplayName}[{index}]: {result.ErrorMessage}";
                        return new ValidationResult(errorMessage);
                    }
                    index++;
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid collection.");
        }
    }
}

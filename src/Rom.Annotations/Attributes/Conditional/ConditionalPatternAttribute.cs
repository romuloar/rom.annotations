using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates the field against a regular expression pattern if another field equals a specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalPatternAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;
        private readonly object _expectedValue;
        private readonly Regex _regex;

        /// <summary>
        /// Creates a new instance of <see cref="ConditionalPatternAttribute"/>.
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property to check.</param>
        /// <param name="expectedValue">The value that the other property must have for validation to apply.</param>
        /// <param name="pattern">The regex pattern to validate against.</param>
        public ConditionalPatternAttribute(string otherPropertyName, 
            object expectedValue, 
            string pattern)
        {
            _otherPropertyName = otherPropertyName ?? throw new ArgumentNullException(nameof(otherPropertyName));
            _expectedValue = expectedValue ?? throw new ArgumentNullException(nameof(expectedValue));
            _regex = new Regex(pattern ?? throw new ArgumentNullException(nameof(pattern)));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
            {
                return new ValidationResult($"Unknown property: {_otherPropertyName}");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            // Check if other property's value equals expected value
            if (Equals(otherValue, _expectedValue))
            {
                // Only validate if current value is a string
                var stringValue = value as string;

                if (stringValue == null)
                {
                    // If value is null or not string, consider invalid (or skip? here we treat null as invalid)
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} must match pattern when {_otherPropertyName} equals {_expectedValue}");
                }

                if (!_regex.IsMatch(stringValue))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} is invalid according to pattern when {_otherPropertyName} equals {_expectedValue}");
                }
            }

            // If condition not met or valid, success
            return ValidationResult.Success;
        }
    }
}

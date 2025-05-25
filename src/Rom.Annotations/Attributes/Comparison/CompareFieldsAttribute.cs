using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates if two fields in the same object are equal or different.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CompareFieldsAttribute : ValidationAttribute
    {
        public string OtherProperty { get; }
        public bool MustBeEqual { get; }

        /// <param name="otherProperty">The name of the other property to compare with.</param>
        /// <param name="mustBeEqual">True if values must be equal; false if must be different.</param>
        public CompareFieldsAttribute(string otherProperty, bool mustBeEqual = true)
        {
            OtherProperty = otherProperty;
            MustBeEqual = mustBeEqual;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherPropertyInfo == null)
                return new ValidationResult($"Unknown property: {OtherProperty}");

            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            bool areEqual = Equals(value, otherValue);

            if (MustBeEqual && !areEqual)
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be equal to {OtherProperty}.");

            if (!MustBeEqual && areEqual)
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be different from {OtherProperty}.");

            return ValidationResult.Success;
        }
    }
}

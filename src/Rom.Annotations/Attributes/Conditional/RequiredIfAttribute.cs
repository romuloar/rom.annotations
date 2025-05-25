using System;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        public string DependentProperty { get; }
        public object TargetValue { get; }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentProperty = validationContext.ObjectType.GetProperty(DependentProperty);

            if (dependentProperty == null)
                return new ValidationResult($"Unknown property: {DependentProperty}");

            var dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance, null);

            bool shouldValidate = Equals(dependentValue, TargetValue);

            if (shouldValidate)
            {
                bool isEmpty = value == null || (value is string s && string.IsNullOrWhiteSpace(s));

                if (isEmpty)
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} is required when {DependentProperty} is '{TargetValue}'");
            }

            return ValidationResult.Success;
        }
    }
}

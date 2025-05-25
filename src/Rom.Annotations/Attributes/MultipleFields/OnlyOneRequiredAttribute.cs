using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rom.Annotations
{
    /// <summary>
    /// Requires exactly one of the specified fields to be filled.
    /// </summary>
    public class OnlyOneRequiredAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public OnlyOneRequiredAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int filledCount = _propertyNames.Count(name =>
            {
                var property = validationContext.ObjectType.GetProperty(name);
                var val = property?.GetValue(validationContext.ObjectInstance);
                return val != null && !(val is string s && string.IsNullOrWhiteSpace(s));
            });

            if (filledCount == 1)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? "Exactly one field must be filled.");
        }
    }
}

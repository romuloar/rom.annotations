using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rom.Annotations
{
    /// <summary>
    /// Requires at least one of the specified fields to be filled.
    /// </summary>
    public class AtLeastOneRequiredAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public AtLeastOneRequiredAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool anyFilled = _propertyNames.Any(name =>
            {
                var property = validationContext.ObjectType.GetProperty(name);
                var val = property?.GetValue(validationContext.ObjectInstance);
                return val != null && !(val is string s && string.IsNullOrWhiteSpace(s));
            });

            return anyFilled ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? "At least one field must be filled.");
        }
    }
}

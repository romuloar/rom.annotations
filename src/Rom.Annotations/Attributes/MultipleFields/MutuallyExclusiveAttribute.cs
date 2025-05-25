using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rom.Annotations
{
    /// <summary>
    /// Ensures that no more than one of the specified fields is filled (mutually exclusive).
    /// </summary>
    public class MutuallyExclusiveAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public MutuallyExclusiveAttribute(params string[] propertyNames)
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

            return filledCount <= 1 ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? "Fields are mutually exclusive.");
        }
    }

}

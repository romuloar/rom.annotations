using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rom.Annotations
{
    /// <summary>
    /// Allows validation via a custom predicate method defined in the containing class.
    /// </summary>
    public class PredicateValidationAttribute : ValidationAttribute
    {
        private readonly string _methodName;

        public PredicateValidationAttribute(string methodName)
        {
            _methodName = methodName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var method = validationContext.ObjectType.GetMethod(_methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
            {
                return new ValidationResult($"Method '{_methodName}' not found.");
            }

            var result = method.Invoke(validationContext.ObjectInstance, new[] { value });
            if (result is bool valid && valid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Predicate validation failed.");
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the value is within the allowed set of values.
    /// </summary>
    public class AllowedValuesAttribute : ValidationAttribute
    {
        private readonly object[] _allowed;

        public AllowedValuesAttribute(params object[] allowed)
        {
            _allowed = allowed;
        }

        public override bool IsValid(object value)
        {
            return value == null || _allowed.Contains(value);
        }
    }
}

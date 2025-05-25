using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the value is NOT within the disallowed set of values.
    /// </summary>
    public class DisallowedValuesAttribute : ValidationAttribute
    {
        private readonly object[] _disallowed;

        public DisallowedValuesAttribute(params object[] disallowed)
        {
            _disallowed = disallowed;
        }

        public override bool IsValid(object value)
        {
            return value == null || !_disallowed.Contains(value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that the string contains the specified substring.
    /// </summary>
    public class StringContainsAttribute : ValidationAttribute
    {
        private readonly string _substring;

        public StringContainsAttribute(string substring)
        {
            _substring = substring;
        }

        public override bool IsValid(object value)
        {
            return value == null || (value is string s && s.Contains(_substring));
        }
    }
}

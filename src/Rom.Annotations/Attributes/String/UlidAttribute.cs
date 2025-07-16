using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Rom.Annotations.Attributes.String
{
    /// <summary>
    /// Validates that the string is a valid ULID.
    /// </summary>
    public class UlidAttribute : ValidationAttribute
    {
        private static readonly Regex UlidRegex = new Regex("^[0123456789ABCDEFGHJKMNPQRSTVWXYZ]{26}$", RegexOptions.Compiled);

        public override bool IsValid(object value)
        {
            if (!(value is string s) || string.IsNullOrWhiteSpace(s))
                return false;
 
            return UlidRegex.IsMatch(s);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rom.Annotations
{
    /// <summary>
    /// Validates that a decimal has at most the specified number of decimal places.
    /// </summary>
    public class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly int _precision;

        public DecimalPrecisionAttribute(int precision)
        {
            _precision = precision;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is decimal dec)
            {
                var scale = BitConverter.GetBytes(decimal.GetBits(dec)[3])[2];
                return scale <= _precision;
            }
            return false;
        }
    }
}

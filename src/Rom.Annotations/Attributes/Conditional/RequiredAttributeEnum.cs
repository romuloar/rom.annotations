using System.ComponentModel.DataAnnotations;
using System;

namespace Rom.Annotations
{
    /// <summary>
    /// Ensures that an enum value is not the default (zero).
    /// </summary>
    public class RequiredAttributeEnum : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var type = value.GetType();
            return type.IsEnum && !value.Equals(Activator.CreateInstance(type));
        }
    }
}

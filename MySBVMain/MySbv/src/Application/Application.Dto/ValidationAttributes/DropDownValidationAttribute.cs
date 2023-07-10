using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.ValidationAttributes
{
    public class DropDownValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return int.Parse(value.ToString()) > 0;
        }
    }

    public class MultiSelectValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && ((List<int>) value).Count > 0;
        }
    }
}
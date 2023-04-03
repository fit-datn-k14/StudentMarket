using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Attributes
{
    public class DuplicateCodeAttribute : ValidationAttribute
    {
        public string ErrorMessage { get; set; }

        public DuplicateCodeAttribute() { }

        public DuplicateCodeAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            // only check string length if empty strings are not allowed
            return true;
        }
    }
}

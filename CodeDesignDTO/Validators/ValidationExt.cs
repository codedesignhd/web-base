using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.Results;

namespace CodeDesign.DTO.Validators
{
    public static class ValidationExt
    {
        public static string GetMessage(this ValidationResult result)
        {
            return string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
        }
    }
}

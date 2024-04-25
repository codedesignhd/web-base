using CodeDesign.Dtos.Accounts;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesign.Dtos.Validators
{
    public interface ICodeDesignValidatorFactory
    {
        ValidationResult Validate<T>(T data) where T : class;
        Task<ValidationResult> ValidateAsync<T>(T data) where T : class;
    }
}

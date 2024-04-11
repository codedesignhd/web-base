using CodeDesign.Dtos.Account;
using CodeDesign.Dtos.Validators;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace CodeDesign.Dtos
{
    public class AppValidator
    {

        private readonly Validators.ICodeDesignValidatorFactory _validatorFactory;

        public AppValidator(Validators.ICodeDesignValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public ValidationResult Validate<T>(T data) where T : class
        {
            IValidator<T> validator = _validatorFactory.GetValidator(data);
            return validator.Validate(data);
        }

        public Task<ValidationResult> ValidateAsync<T>(T data) where T : class
        {
            IValidator<T> validator = _validatorFactory.GetValidator(data);
            return validator.ValidateAsync(data);
        }
    }
}

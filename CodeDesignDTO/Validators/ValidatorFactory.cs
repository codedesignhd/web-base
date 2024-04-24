using CodeDesign.Dtos.Accounts;
using FluentValidation.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeDesign.Dtos.Auth;

namespace CodeDesign.Dtos.Validators
{
    public class ValidatorFactory : ICodeDesignValidatorFactory
    {
        private IValidator<T> getValidator<T>(T _) where T : class
        {
            if (typeof(T) == typeof(RegisterUserRequest))
            {
                return (IValidator<T>)new RegisterValidator();
            }
            else if (typeof(T) == typeof(ChangePwdRequest))
            {
                return (IValidator<T>)new ChangePasswordValidator();
            }
            else if (typeof(T) == typeof(AuthRequest))
            {
                return (IValidator<T>)new AuthRequestValidator();
            }

            // Add more cases for other types if needed

            throw new InvalidOperationException($"Validator for type {typeof(T)} not found.");
        }

        public ValidationResult Validate<T>(T data) where T : class
        {
            IValidator<T> validator = getValidator(data);
            return validator.Validate(data);
        }

        public Task<ValidationResult> ValidateAsync<T>(T data) where T : class
        {
            IValidator<T> validator = getValidator(data);
            return validator.ValidateAsync(data);
        }
    }
}

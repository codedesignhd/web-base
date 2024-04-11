using CodeDesign.Dtos.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Validators
{
    public interface ICodeDesignValidatorFactory
    {
        IValidator<T> GetValidator<T>(T data) where T : class;
    }

    public class ValidatorFactory : ICodeDesignValidatorFactory
    {
        public IValidator<T> GetValidator<T>(T data) where T : class
        {
            if (typeof(T) == typeof(RegisterUserRequest))
            {
                return (IValidator<T>)new RegisterValidator();
            }
            else if (typeof(T) == typeof(ChangePwdRequest))
            {
                return (IValidator<T>)new ChangePasswordValidator();
            }
            // Add more cases for other types if needed

            throw new InvalidOperationException($"Validator for type {typeof(T)} not found.");
        }
    }
}

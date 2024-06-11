using FluentValidation.Results;
using FluentValidation;
using System;
using System.Threading.Tasks;
using CodeDesignDtos.Auth;
using CodeDesignDtos.Accounts;
using CodeDesignDtos.Packages;
using CodeDesignDtos.Validators.Auth;
using CodeDesignDtos.Validators.Packages;
using CodeDesignDtos.Validators.Accounts;

namespace CodeDesignDtos.Validators
{
    public class ValidatationFactory : IValidatationFactory
    {
        private IValidator<T> getValidator<T>(T _) where T : class
        {
            Type type = typeof(T);
            if (type == typeof(RegisterUserRequest))
            {
                return (IValidator<T>)RegisterValidator.Instance;
            }
            else if (type == typeof(ChangePwdRequest))
            {
                return (IValidator<T>)ChangePasswordValidator.Instance;
            }
            else if (type == typeof(AuthRequest))
            {
                return (IValidator<T>)AuthValidator.Instance;
            }
            else if (type == typeof(UpdateUserInfoRequest))
            {
                return (IValidator<T>)UpdateUserInfoValidator.Instance;
            }
            else if (type == typeof(CreatePackageRequest))
            {
                return (IValidator<T>)CreatePackageValidator.Instance;
            }


            // Add more cases for other types if needed

            throw new InvalidOperationException($"Validator for type {type} not found.");
        }

        public ValidationResult Validate<T>(T data) where T : class
        {
            IValidator<T> validator = getValidator(data);
            return validator.Validate(data);
        }

        public async Task<ValidationResult> ValidateAsync<T>(T data) where T : class
        {
            IValidator<T> validator = getValidator(data);
            return await validator.ValidateAsync(data);
        }
    }
}

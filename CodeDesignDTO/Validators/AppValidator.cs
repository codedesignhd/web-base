using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace CodeDesign.Dtos.Validators
{
    public class AppValidator
    {

        private readonly ICodeDesignValidatorFactory _validatorFactory;

        public AppValidator(ICodeDesignValidatorFactory validatorFactory)
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

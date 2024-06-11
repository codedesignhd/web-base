using FluentValidation.Results;
using System.Threading.Tasks;

namespace CodeDesignDtos.Validators
{
    public interface IValidatationFactory
    {
        ValidationResult Validate<T>(T data) where T : class;
        Task<ValidationResult> ValidateAsync<T>(T data) where T : class;
    }
}

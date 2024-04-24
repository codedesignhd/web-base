using CodeDesign.Dtos.Accounts;
using CodeDesign.Dtos.Auth;
using CodeDesign.Utilities.Constants;
using FluentValidation;
namespace CodeDesign.Dtos.Validators
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(x => x.username)
                .NotNull()
                    .WithMessage("Tên tài khoản không được để trống")
                .MinimumLength(6)
                    .WithMessage("Tên tài khoản tối thiểu 6 kí tự")
                .Must(username => RegexConst.RegXUsername.IsMatch(username))
                    .WithMessage("Tên tài khoản chỉ được chứa số và chữ cái");
            RuleFor(x => x.password).MinimumLength(6);
        }
    }
}

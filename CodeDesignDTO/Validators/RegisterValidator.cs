using CodeDesign.Dtos.Accounts;
using CodeDesign.Utilities.Constants;
using FluentValidation;
namespace CodeDesign.Dtos.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.username)
                .NotNull()
                    .WithMessage("Tên tài khoản không được để trống")
                .MinimumLength(4)
                    .WithMessage("Tên tài khoản tối thiểu 4 kí tự")
                .Must(username => RegexConst.RegXUsername.IsMatch(username))
                    .WithMessage("Tên tài khoản chỉ được chứa số và chữ cái");
            //RuleFor(x => x.fullname).NotNull();
            RuleFor(x => x.password).MinimumLength(6);
            RuleFor(x => x.email).EmailAddress();
        }
        private static RegisterValidator _instance;

        public static RegisterValidator Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new RegisterValidator();
                }
                return _instance;
            }
        }
    }
}

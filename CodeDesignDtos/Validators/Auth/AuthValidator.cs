using CodeDesignDtos.Auth;
using CodeDesignUtilities.Constants;
using FluentValidation;
namespace CodeDesignDtos.Validators.Auth
{
    public class AuthValidator : AbstractValidator<AuthRequest>
    {
        public AuthValidator()
        {
            RuleFor(x => x.username)
                .NotNull()
                    .WithMessage("Tên tài khoản không được để trống")
                .MinimumLength(4)
                    .WithMessage("Tên tài khoản tối thiểu 6 kí tự")
                .Must(username => RegexConst.RegXUsername.IsMatch(username))
                    .WithMessage("Tên tài khoản chỉ được chứa số và chữ cái");
            RuleFor(x => x.password).MinimumLength(6);
        }
        private static AuthValidator _instance;

        public static AuthValidator Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new AuthValidator();
                }
                return _instance;
            }
        }
    }
}

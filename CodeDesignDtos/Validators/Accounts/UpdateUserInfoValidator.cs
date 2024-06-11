using CodeDesignDtos.Accounts;
using CodeDesignUtilities;
using FluentValidation;

namespace CodeDesignDtos.Validators.Accounts
{
    public class UpdateUserInfoValidator : AbstractValidator<UpdateUserInfoRequest>
    {
        public UpdateUserInfoValidator()
        {

            RuleFor(x => x.fullname)
                .NotNull()
                    .WithMessage("Họ tên không được để trống");

            RuleFor(x => x.dob)
               .Must(x => DateTimeUtils.IsValidDate(x))
               .WithMessage("Ngày sinh không hợp lệ");
        }

        private static UpdateUserInfoValidator _instance;

        public static UpdateUserInfoValidator Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new UpdateUserInfoValidator();
                }
                return _instance;
            }
        }


    }
}

using System;
using System.Text.RegularExpressions;
using CodeDesign.DTO.Dtos.TaiKhoan;
using FluentValidation;
namespace CodeDesign.DTO.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.username)
                .NotNull().WithMessage("Tên tài khoản không được để trống")
                .MinimumLength(4).WithMessage("Tên tài khoản tối thiểu 6 kí tự")
                .Must(username => Regex.IsMatch(username, Utils.Const.RGX_USERNAME, RegexOptions.None, TimeSpan.FromSeconds(5))).WithMessage("Tên tài khoản chỉ được chứa số và chữ cái");
            //RuleFor(x => x.fullname).NotNull();
            RuleFor(x => x.password).MinimumLength(6);
            RuleFor(x => x.email).EmailAddress();
        }
    }
}

using CodeDesign.Dtos.Accounts;
using CodeDesign.Utilities;
using CodeDesign.Utilities.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Validators
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

using CodeDesignDtos.Accounts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignDtos.Validators.Accounts
{
    public class ChangePasswordValidator : AbstractValidator<ChangePwdRequest>
    {
        public ChangePasswordValidator()
        {

            RuleFor(x => x.old_password)
                .NotEmpty()
                .WithMessage("Old password is invalid");

            RuleFor(x => x.new_password)
                .NotEmpty()
                .WithMessage("Password is invalid");

            RuleFor(x => x.re_new_password)
               .Equal(x => x.new_password)
               .WithMessage("Password are not the same");
        }
        private static ChangePasswordValidator _instance;

        public static ChangePasswordValidator Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new ChangePasswordValidator();
                }
                return _instance;
            }
        }

    }
}

using CodeDesign.Dtos.Accounts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePwdRequest>
    {
        public ChangePasswordValidator()
        {

            RuleFor(x => x.OldPasword)
                .NotEmpty()
                .WithMessage("Old password is invalid");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Password is invalid");

            RuleFor(x => x.ReNewPassword)
               .Equal(x => x.NewPassword)
               .WithMessage("Password are not the same");


        }
    }
}

﻿using CodeDesign.Dtos.Accounts;
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
    }
}

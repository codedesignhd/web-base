using CodeDesign.Dtos.Packages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Validators
{
    internal class CreatePackageValidator : AbstractValidator<CreatePackageRequest>
    {

        public CreatePackageValidator()
        {

        }
        private static CreatePackageValidator _instance;

        public static CreatePackageValidator Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new CreatePackageValidator();
                }
                return _instance;
            }
        }
    }
}

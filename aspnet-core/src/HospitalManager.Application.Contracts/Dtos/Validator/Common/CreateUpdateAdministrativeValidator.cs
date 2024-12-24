using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HospitalManager.Dtos.Request.CreateUpdate.Common;

namespace HospitalManager.Dtos.Validator.Common;

public class CreateUpdateAdministrativeValidator : AbstractValidator<CreateUpdateAdministrative>
{
    public CreateUpdateAdministrativeValidator()
    {
        Include(new BaseCreateUpdateValidator());

        RuleFor(x => x.AdministrativeLevel)
            .NotEmpty().WithMessage("Cấp hành chính là bắt buộc.");

    }
}
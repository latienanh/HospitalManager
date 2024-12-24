using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Validator.Common;

namespace HospitalManager.Dtos.Validator;

public class WardDtoValidator : AbstractValidator<CreateUpdateWardDto>
{
    public WardDtoValidator()
    {
        Include(new CreateUpdateAdministrativeValidator());
        RuleFor(x => x.ProvinceCode)
            .NotEmpty().WithMessage("Mã tỉnh là bắt buộc");
        RuleFor(x => x.DistrictCode)
            .NotEmpty().WithMessage("Mã huyện là bắt buộc");
    }

}
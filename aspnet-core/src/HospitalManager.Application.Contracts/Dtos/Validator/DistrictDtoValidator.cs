using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Validator.Common;

namespace HospitalManager.Dtos.Validator;

public class DistrictDtoValidator : AbstractValidator<CreateUpdateDistrictDto>
{
    public DistrictDtoValidator()
    {
        Include(new CreateUpdateAdministrativeValidator());
        RuleFor(x => x.ProvinceCode)
            .NotEmpty().WithMessage("Mã tỉnh là bắt buộc");
                
    }
}
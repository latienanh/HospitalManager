using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HospitalManager.Dtos.Request.CreateUpdate;

namespace HospitalManager.Dtos.Validator;

public class UserHospitalDtoValidator : AbstractValidator<CreateUpdateUserHospitalDto>
{
    public UserHospitalDtoValidator()
    {
        RuleFor(x => x.HospitalId)
            .NotEmpty().WithMessage("Mã bệnh viện là bắt buộc");
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Mã người dùng là bắt buộc");
    }
}
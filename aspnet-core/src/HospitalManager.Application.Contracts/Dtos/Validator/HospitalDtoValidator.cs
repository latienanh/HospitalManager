using FluentValidation;
using HospitalManager.Dtos.Request.CreateUpdate;
using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Validator.Common;

namespace HospitalManager.Dtos.Validator;

public class HospitalDtoValidator : AbstractValidator<CreateUpdateHospitalDto>
{
    public HospitalDtoValidator() {
        Include(new BaseCreateUpdateValidator());
        RuleFor(x => x.ProvinceCode)
            .NotEmpty().WithMessage("Mã tỉnh là bắt buộc");
        RuleFor(x => x.WardCode)
            .NotEmpty().WithMessage("Mã xã là bắt buộc");
        RuleFor(x => x.DistrictCode)
            .NotEmpty().WithMessage("Mã huyện là bắt buộc");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ là bắt buộc")
            .MaximumLength(30).WithMessage("Địa chỉ nhập ít hơn 30 kí tự");
    }
}
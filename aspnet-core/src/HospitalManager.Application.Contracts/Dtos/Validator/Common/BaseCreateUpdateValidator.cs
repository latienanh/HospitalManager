using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HospitalManager.Dtos.Request.CreateUpdate.Common;

namespace HospitalManager.Dtos.Validator.Common;

public class BaseCreateUpdateValidator : AbstractValidator<BaseCreateUpdate>
{
    public BaseCreateUpdateValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã là bắt buộc.")
            .GreaterThan(0).WithMessage("Mã phải lớn hơn 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên là bắt buộc.")
            .MaximumLength(100).WithMessage("Tên không được dài hơn 100 ký tự.");
    }
}
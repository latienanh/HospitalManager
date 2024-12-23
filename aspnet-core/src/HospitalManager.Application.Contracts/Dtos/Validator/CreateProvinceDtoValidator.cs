using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HospitalManager.Dtos.CreateUpdate;

namespace HospitalManager.Dtos.Validator
{
    public class CreateProvinceDtoValidator : AbstractValidator<CreateUpdateProvinceDto>
    {
        public CreateProvinceDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã là bắt buộc.")
                .GreaterThan(0).WithMessage("Mã phải lớn hơn 0.");

            RuleFor(x => x.Name)
                .Must(y=>!string.IsNullOrEmpty(y)).WithMessage("helo")
                .NotEmpty().WithMessage("Tên là bắt buộc.")
                .MaximumLength(100).WithMessage("Tên không được dài hơn 100 ký tự.");

            RuleFor(x => x.AdministrativeLevel)
                .NotEmpty().WithMessage("Cấp hành chính là bắt buộc.");

            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Ghi chú là bắt buộc");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Validator.Common;

namespace HospitalManager.Dtos.Validator;

public class ProvinceDtoValidator : AbstractValidator<CreateUpdateProvinceDto>
{
    public ProvinceDtoValidator()
    {
        Include(new CreateUpdateAdministrativeValidator());
    }

}
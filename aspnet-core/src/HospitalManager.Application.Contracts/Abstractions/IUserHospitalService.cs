﻿using System;
using System.Collections.Generic;
using System.Text;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HospitalManager.Abstractions;

public interface IUserHospitalService : ICrudAppService<UserHospitalDto, int, PagedAndSortedResultRequestDto, CreateUpdateUserHospitalDto>
{
}
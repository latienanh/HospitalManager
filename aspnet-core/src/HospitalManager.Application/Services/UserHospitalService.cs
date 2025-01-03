﻿using AutoMapper.Internal.Mappers;
using HospitalManager.Abstractions;
using HospitalManager.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Microsoft.AspNetCore.Authorization;

namespace HospitalManager.Services
{
    [Authorize("UserHospital_Manager")]
    public class UserHospitalService(
    IRepository<UserHospital, int> repository
)
    : CrudAppService<UserHospital, UserHospitalDto, int, PagedAndSortedResultRequestDto, CreateUpdateUserHospitalDto>(
        repository), IUserHospitalService
    {
        [Authorize("UserHospital_Delete")]
        public override async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }
        public async Task<int?> GetHospitalByUserId(Guid? userId)
        {
            if (userId == null)
            {
                return null;
            }
            var result = await repository.FirstOrDefaultAsync(x => x.UserId == userId);
            if (result == null)
                return null;
            return result?.HospitalId;
        }
    }
}

using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Request.GetPaging;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using HospitalManager.Services;
using Volo.Abp.Users;

namespace PatientManager.Services
{
    public class PatientService(
        IRepository<Patient, int> repository,
        IUserHospitalService userHospitalService,
        ICurrentUser currentUser,
        IPatientDapperRepository PatientDapperRepository
    )
        : CrudAppService<Patient, PatientDto, int, PagedAndSortedResultRequestDto, CreateUpdatePatientDto>(
            repository), IPatientService
    {
        [HttpPost]
        public async Task<GetPagingResponse<PatientDto>> GetPatientDapperListAsync([FromBody] BaseGetPagingRequest request)
        {
            var hospitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            var patients = await PatientDapperRepository.GetPagingAsync(request.Index, request.Size, $"WHERE IsDeleted =FALSE AND HospitalId = {hospitalId} ");
            var totalPage = await PatientDapperRepository.GetCountAsync(request.Size);
            var mappedPatients = ObjectMapper.Map<List<Patient>, List<PatientDto>>(patients);
            var result = new GetPagingResponse<PatientDto>
            {
                Data = mappedPatients,
                TotalPage = totalPage
            };
            return result;
        }
        public override async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }


        public override async Task<PatientDto> CreateAsync(CreateUpdatePatientDto input)
        {
            input.HospitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
            if (checkCode != null)
            {
                throw new BusinessException()
                    .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
            }
            return await base.CreateAsync(input);
        }
        public override async Task<PatientDto> UpdateAsync(int id, CreateUpdatePatientDto input)
        {
            var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code && x.Id != id);
            if (checkCode != null)
            {
                throw new BusinessException()
                    .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
            }
            return await base.UpdateAsync(id, input);
        }
    }
}

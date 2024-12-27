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
using HospitalManager.Hub;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using HospitalManager.Services;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Authorization;

namespace PatientManager.Services
{
    [Authorize("Patient_Manager")]
    public class PatientService(
        IRepository<Patient, int> repository,
        IUserHospitalService userHospitalService,
        ICurrentUser currentUser,
        IPatientDapperRepository PatientDapperRepository,
        IHubContext<NotificationHub> hubContext

    )
        : CrudAppService<Patient, PatientDto, int, PagedAndSortedResultRequestDto, CreateUpdatePatientDto>(
            repository), IPatientService
    {
        [HttpPost]
        [Authorize("Patient_GetPaging")]
        public async Task<GetPagingResponse<PatientDto>> GetPatientDapperListAsync([FromBody] BaseGetPagingRequest request)
        {
            var resultHosspitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            if (resultHosspitalId == null)
            {
                throw new BusinessException()
                    .WithData("message", $"Bạn chưa có bệnh viện nào ib admin để được vào bệnh viện");

            }
            string whereClause = $"WHERE IsDeleted =FALSE AND HospitalId = {resultHosspitalId} ";
            
            var patients = await PatientDapperRepository.GetPagingAsync(request.Index, request.Size, whereClause);
            var totalPage = await PatientDapperRepository.GetCountAsync(request.Size, whereClause);
            var mappedPatients = ObjectMapper.Map<List<Patient>, List<PatientDto>>(patients);
            var result = new GetPagingResponse<PatientDto>
            {
                Data = mappedPatients,
                TotalPage = totalPage
            };
            return result;
        }
        [Authorize("Patient_Delete")]
        public override async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }

        [Authorize("Patient_Create")]
        public override async Task<PatientDto> CreateAsync(CreateUpdatePatientDto input)
        {
            var test = currentUser.Roles != null && currentUser.Roles.Contains("UserHospital");
            var resultHospitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            if (resultHospitalId == null)
            {
                throw new BusinessException()
                    .WithData("message", $"Bạn chưa có bệnh viện nào ib admin để được vào bệnh viện");

            }

            input.HospitalId = resultHospitalId;
            var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
            if (checkCode != null)
            {
                throw new BusinessException()
                    .WithData("message", $"Bạn chưa có bệnh viện nào ib admin để được vào bệnh viện");
            }

            var result = await base.CreateAsync(input);
            await hubContext.Clients.Group("Admin").SendAsync("ReceiveNotification",
                $"Đã thêm mới 1 bệnh nhân là {result.Name} vào lúc {result.CreationTime}.");
            return result ;
        }
        [Authorize("Patient_Update")]
        public override async Task<PatientDto> UpdateAsync(int id, CreateUpdatePatientDto input)
        {
            var resultHospitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            if (resultHospitalId == null)
            {
                     throw new BusinessException()
                        .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
                
            }
                  
            input.HospitalId = resultHospitalId;
            var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code && x.Id != id);
            if (checkCode != null)
            {
                throw new BusinessException()
                    .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
            }
            return await base.UpdateAsync(id, input);
        }
        public async Task<IEnumerable<HospitalPatientCountDto>> HospitalPatientCountReport()
        {

            var result = await PatientDapperRepository.HospitalPatientCountReportDapper();

            return result;
        }
    
        public async Task<IEnumerable<ProvincePatientCountDto>> ProvincePatientCountReport()
        {
           var result = await PatientDapperRepository.ProvincePatientCountReportDapper();
            return result;
        }


    }
}

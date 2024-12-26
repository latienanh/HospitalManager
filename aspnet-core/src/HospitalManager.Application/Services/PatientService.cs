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

namespace PatientManager.Services
{
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
        public async Task<GetPagingResponse<PatientDto>> GetPatientDapperListAsync([FromBody] BaseGetPagingRequest request)
        {
            var resultHosspitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            if (resultHosspitalId == null)
            {
                throw new BusinessException()
                    .WithData("message", $"Bạn chưa có bệnh viện nào ib admin để được vào bệnh viện");

            }
            var patients = await PatientDapperRepository.GetPagingAsync(request.Index, request.Size, $"WHERE IsDeleted =FALSE AND HospitalId = {resultHosspitalId} ");
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
            var test = currentUser.Roles != null && currentUser.Roles.Contains("UserHospital");
            var resultHosspitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            if (resultHosspitalId == null)
            {
                throw new BusinessException()
                    .WithData("message", $"Bạn chưa có bệnh viện nào ib admin để được vào bệnh viện");

            }

            input.HospitalId = resultHosspitalId;
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
        public override async Task<PatientDto> UpdateAsync(int id, CreateUpdatePatientDto input)
        {
            var resultHosspitalId = await userHospitalService.GetHospitalByUserId(currentUser.Id);
            if (resultHosspitalId == null)
            {
                     throw new BusinessException()
                        .WithData("message", $"Mã: {input.Code} đã tồn tại trong hệ thống");
                
            }
                  
            input.HospitalId = resultHosspitalId;
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

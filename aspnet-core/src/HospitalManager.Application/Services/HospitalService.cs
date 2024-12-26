using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;

namespace HospitalManager.Services
{
    public class HospitalService(
        IRepository<Hospital, int> repository,
        IHospitalDapperRepository hospitalDapperRepository
    )
        : CrudAppService<Hospital, HospitalDto, int, PagedAndSortedResultRequestDto, CreateUpdateHospitalDto>(
            repository), IHospitalService
    {
        [HttpPost]
        public async Task<GetPagingResponse<HospitalDto>> GetHospitalDapperListAsync([FromBody]BaseGetPagingRequest request)
        {
            var hospitals = await hospitalDapperRepository.GetPagingAsync(request.Index, request.Size, $"WHERE IsDeleted =FALSE");
            var totalPage = await hospitalDapperRepository.GetCountAsync(request.Size);
            var mappedHospitals = ObjectMapper.Map<List<Hospital>, List<HospitalDto>>(hospitals);
            var result = new GetPagingResponse<HospitalDto>
            {
                Data = mappedHospitals,
                TotalPage = totalPage
            };
            return result;
        }
        [HttpPost]
        public async Task<GetPagingResponse<UserDto>> GetUserNotInHospitalDapperListAsync([FromBody] BaseGetPagingRequest request)
        {
            var users = await hospitalDapperRepository.GetUserNotInHospital(request.Index, request.Size);
            var totalPage = await hospitalDapperRepository.GetCountUserNotInHospitalAsync(request.Size);
            var mappedUsers = ObjectMapper.Map<IEnumerable<IdentityUser>, List<UserDto>>(users);
            var result = new GetPagingResponse<UserDto>
            {
                Data = mappedUsers,
                TotalPage = totalPage
            };
            return result;
        }
        public override async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }
     

        public override async Task<HospitalDto> CreateAsync(CreateUpdateHospitalDto input)
        {
            try
            {
                var checkCode = await Repository.FirstOrDefaultAsync(x => x.Code == input.Code);
                if (checkCode != null)
                {
                    throw new BusinessException()
                        .WithData("message", "helo");
                }
                return await base.CreateAsync(input);
            }
           
            catch (Exception ex)
            {
                // Xử lý các lỗi khác nếu cần
                throw new ApplicationException("An unexpected error occurred while creating the hospital.", ex);
            }
        
        }
        public override async Task<HospitalDto> UpdateAsync(int id, CreateUpdateHospitalDto input)
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

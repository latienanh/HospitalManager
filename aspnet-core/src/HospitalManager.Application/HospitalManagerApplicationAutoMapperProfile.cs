using AutoMapper;
using HospitalManager.Dtos;
using HospitalManager.Dtos.CreateUpdate;
using HospitalManager.Entities;

namespace HospitalManager;

public class HospitalManagerApplicationAutoMapperProfile : Profile
{
    public HospitalManagerApplicationAutoMapperProfile()
    {
        CreateMap<Province, ProvinceDto>().ReverseMap();
        CreateMap<Province, CreateUpdateProvinceDto>().ReverseMap();
    }
}

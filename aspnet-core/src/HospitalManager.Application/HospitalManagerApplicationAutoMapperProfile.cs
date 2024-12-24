using AutoMapper;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;

namespace HospitalManager;

public class HospitalManagerApplicationAutoMapperProfile : Profile
{
    public HospitalManagerApplicationAutoMapperProfile()
    {
        CreateMap<Province, ProvinceDto>().ReverseMap();
        CreateMap<Province, CreateUpdateProvinceDto>().ReverseMap();
        CreateMap<District, DistrictDto>().ReverseMap();
        CreateMap<District, CreateUpdateDistrictDto>().ReverseMap();
        CreateMap<Ward, WardDto>().ReverseMap();
        CreateMap<Ward, CreateUpdateWardDto>().ReverseMap();
    }
}

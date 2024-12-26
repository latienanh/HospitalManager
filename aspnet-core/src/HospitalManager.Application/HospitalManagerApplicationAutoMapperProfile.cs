using AutoMapper;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using Volo.Abp.Identity;

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
        CreateMap<Hospital, CreateUpdateHospitalDto>().ReverseMap();
        CreateMap<Hospital, HospitalDto>().ReverseMap();
        CreateMap<Patient, CreateUpdatePatientDto>().ReverseMap();
        CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<UserHospital, UserHospitalDto>().ReverseMap();
        CreateMap<UserHospital, CreateUpdateUserHospitalDto>().ReverseMap();
        CreateMap<UserDto, IdentityUser>().ReverseMap();
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using HospitalManager.Services;
using Moq;
using NSubstitute;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace HospitalManager.Service.Test
{
    public class ProvinceServiceTests
    {
        public Mock<IRepository<Province, int>> MockProvinceRepository { get; }
        public Mock<IProvinceDapperRepository> MockProvinceDapperRepository { get; }
        public Mock<ExcelService> MockExcelService { get; }
        public ProvinceAppService ProvinceAppService { get; }

        public Mock<IObjectMapper> MockObjectMapper { get; }

        public ProvinceServiceTests()
        {
            MockProvinceRepository = new Mock<IRepository<Province, int>>();
            MockProvinceDapperRepository = new Mock<IProvinceDapperRepository>();
            MockExcelService = new Mock<ExcelService>();
            MockObjectMapper = new Mock<IObjectMapper>();

            // Khởi tạo ProvinceAppService với các mock đã tạo
            ProvinceAppService = new ProvinceAppService(
                MockProvinceRepository.Object,
                MockProvinceDapperRepository.Object,
                MockExcelService.Object,
                MockObjectMapper.Object
            );
        }
        [Fact]
        public async Task GetProvinceDapperListAsync_Should_Return_Provinces()
        {
            // Arrange
            var mockProvinces = new List<Province>
            {
                new Province {  Code = 1, Name = "Province 1",AdministrativeLevel = "Tỉnh"},
                new Province {  Code = 2, Name = "Province 2" ,AdministrativeLevel = "Tỉnh"}
            };
            var mockCount = 2;
            var mockProvincesDto = new List<ProvinceDto>
            {
                new ProvinceDto {  Code = 1, Name = "Province 1",AdministrativeLevel = "Tỉnh"},
                new ProvinceDto {  Code = 2, Name = "Province 2" ,AdministrativeLevel = "Tỉnh"}
            };

            var request = new BaseGetPagingRequest { Index = 0, Size = 1 };
            MockProvinceDapperRepository.Setup(repo => repo.GetPagingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(mockProvinces);
            MockProvinceDapperRepository.Setup(repo => repo.GetCountAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(2);
            MockObjectMapper.Setup(m => m.Map<List<Province>, List<ProvinceDto>>(It.IsAny<List<Province>>()))
                .Returns(mockProvincesDto);
            // Act
            var result = await ProvinceAppService.GetProvinceDapperListAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalPage);
            Assert.Equal(2, result.Data.Count);
        }
    }
}

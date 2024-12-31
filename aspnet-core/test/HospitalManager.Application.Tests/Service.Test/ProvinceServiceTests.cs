using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Common;
using HospitalManager.Dtos.Request.CreateUpdate;
using HospitalManager.Dtos.Response;
using HospitalManager.Entities;
using HospitalManager.Services;
using Microsoft.AspNetCore.Http;
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
        public Mock<IRepository<Province, int>> _mockProvinceRepository { get; }
        public Mock<IProvinceDapperRepository> _mockProvinceDapperRepository { get; }
        public Mock<ExcelService> _mockExcelService { get; }
        public ProvinceAppService _provinceAppService { get; }

        public Mock<IObjectMapper> _mockObjectMapper { get; }

        public ProvinceServiceTests()
        {
            _mockProvinceRepository = new Mock<IRepository<Province, int>>();
            _mockProvinceDapperRepository = new Mock<IProvinceDapperRepository>();
            _mockExcelService = new Mock<ExcelService>();
            _mockObjectMapper = new Mock<IObjectMapper>();

            // Khởi tạo ProvinceAppService với các mock đã tạo
            _provinceAppService = new ProvinceAppService(
                _mockProvinceRepository.Object,
                _mockProvinceDapperRepository.Object,
                _mockExcelService.Object,
                _mockObjectMapper.Object
            );
        }
        [Fact]
        public async Task GetProvinceDapperListAsync_Should_Return_Provinces()
        {
            // Arrange
            var request = new BaseGetPagingRequest { Index = 1, Size = 10 };
            var provinces = new List<Province> { new Province { Code = 1, Name = "Province1" } };
            var mappedProvinces = new List<ProvinceDto> { new ProvinceDto { Code = 1, Name = "Province1" } };

            _mockProvinceDapperRepository.Setup(repo => repo.GetPagingAsync(request.Index, request.Size, It.IsAny<string>()))
                .ReturnsAsync(provinces);
            _mockProvinceDapperRepository.Setup(repo => repo.GetCountAsync(request.Size, It.IsAny<string>()))
                .ReturnsAsync(1);
            _mockObjectMapper.Setup(mapper => mapper.Map<List<Province>, List<ProvinceDto>>(provinces))
                .Returns(mappedProvinces);

            // Act
            var result = await _provinceAppService.GetProvinceDapperListAsync(request);

            // Assert
            Assert.Equal(1, result.TotalPage);
            Assert.Equal(mappedProvinces, result.Data);
        }
        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync()
        {
            // Arrange
            int id = 1;

            // Act
            await _provinceAppService.DeleteAsync(id);

            // Assert
            _mockProvinceRepository.Verify(repo =>  repo.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProvince()
        {
            // Arrange
            var createDto = new CreateUpdateProvinceDto { Code = 123, Name = "Province1" };
            _mockProvinceRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Func<Province, bool>>()))
                .ReturnsAsync((Province)null);

            // Act
            var result = await _provinceAppService.CreateAsync(createDto);

            // Assert
            _mockProvinceRepository.Verify(repo => repo.InsertAsync(It.IsAny<Province>(), true), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProvince()
        {
            // Arrange
            int id = 1;
            var updateDto = new CreateUpdateProvinceDto { Code = 123, Name = "Province1" };
            _mockProvinceRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Func<Province, bool>>()))
                .ReturnsAsync((Province)null);

            // Act
            var result = await _provinceAppService.UpdateAsync(id, updateDto);

            // Assert
            _mockProvinceRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Province>(), true), Times.Once);
        }
        [Fact]
        public async Task ImportExcel_ShouldReturnTrue()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.xlsx";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Act
            var result = await _provinceAppService.ImportExcel(fileMock.Object, false);

            // Assert
            Assert.True(result);
        }
    }
}

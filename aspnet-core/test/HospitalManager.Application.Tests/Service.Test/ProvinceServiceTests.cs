using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
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
using OfficeOpenXml;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace HospitalManager.Service.Test
{
    public class ProvinceServiceTests
    {
        //private readonly Mock<IRepository<Province, int>> _repositoryMock;
        //private readonly Mock<IProvinceDapperRepository> _provinceDapperRepositoryMock;
        //private readonly Mock<ExcelService> _excelServiceMock;
        //private readonly Mock<IObjectMapper> _objectMapperMock;
        //private readonly ProvinceAppService _service;

        //public ProvinceServiceTests()
        //{
        //    _repositoryMock = new Mock<IRepository<Province, int>>();
        //    _provinceDapperRepositoryMock = new Mock<IProvinceDapperRepository>();
        //    _excelServiceMock = new Mock<ExcelService>();
        //    _objectMapperMock = new Mock<IObjectMapper>();
        //    _service = new ProvinceAppService(
        //        _repositoryMock.Object,
        //        _provinceDapperRepositoryMock.Object,
        //        _excelServiceMock.Object,
        //        _objectMapperMock.Object
        //    );
        //}

        //[Fact]
        //public async Task GetProvinceDapperListAsync_ReturnsPagedData()
        //{
        //    // Arrange
        //    var request = new BaseGetPagingRequest { Index = 0, Size = 1 };
        //    var provinces = new List<Province> { new Province { Code = 1, Name = "Province A" } };
        //    var totalPage = 1;

        //    _provinceDapperRepositoryMock.Setup(r => r.GetPagingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
        //        .ReturnsAsync(provinces);
        //    _provinceDapperRepositoryMock.Setup(r => r.GetCountAsync(It.IsAny<int>(), It.IsAny<string>()))
        //        .ReturnsAsync(totalPage);
        //    _objectMapperMock.Setup(m => m.Map<List<Province>, List<ProvinceDto>>(It.IsAny<List<Province>>()))
        //        .Returns(new List<ProvinceDto> { new ProvinceDto { Code = 1, Name = "Province A" } });

        //    // Act
        //    var result = await _service.GetProvinceDapperListAsync(request);

        //    // Assert
        //    Assert.Equal(1, result.TotalPage);
        //    Assert.Single(result.Data);
        //    Assert.Equal("Province A", result.Data.First().Name);
        //}

        //[Fact]
        //public async Task GetProvinceDapperListAsync_ReturnsEmptyData_WhenNoDataFound()
        //{
        //    // Arrange
        //    var request = new BaseGetPagingRequest { Index = 1, Size = 10 };
        //    var provinces = new List<Province>();
            
        //    var totalPage = 0;

        //    _provinceDapperRepositoryMock.Setup(r => r.GetPagingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
        //        .ReturnsAsync(provinces);
        //    _provinceDapperRepositoryMock.Setup(r => r.GetCountAsync(It.IsAny<int>(), It.IsAny<string>()))
        //        .ReturnsAsync(totalPage);
        //    _objectMapperMock.Setup(r => r.Map<List<Province>,List<ProvinceDto>>(provinces))
        //        .ReturnsAsync(totalPage);
        //    // Act
        //    var result = await _service.GetProvinceDapperListAsync(request);

        //    // Assert
        //    Assert.Equal(0, result.TotalPage);
        //    Assert.Equal(null,result.Data);
        //}
        //[Fact]
        //public async Task CreateAsync_ThrowsBusinessException_WhenCodeAlreadyExists()
        //{
        //    // Arrange
        //    var input = new CreateUpdateProvinceDto { Code = 123, Name = "Province A" };
        //    _repositoryMock.Setup(r => r.FindAsync(x=>x.Code==input.Code,true,default))
        //        .ReturnsAsync(new Province { Code = 123 });

        //    // Act & Assert
        //    var exception = await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(input));
        //    Assert.Equal("Mã: 123 đã tồn tại trong hệ thống", exception.Message);
        //}

        //[Fact]
        //public async Task CreateAsync_CreatesProvinceSuccessfully_WhenCodeDoesNotExist()
        //{
        //    // Arrange
        //    var input = new CreateUpdateProvinceDto { Code = 123, Name = "Province A" };
        //    _repositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Province, bool>>>(), true, It.IsAny<CancellationToken>()))
        //        .ReturnsAsync((Province)null); // No existing province with this code
        //    _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Province>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns((Task<Province>)Task.CompletedTask);
         
        //    // Act
        //    var result = await _service.CreateAsync(input);

        //    // Assert
        //    _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Province>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.Equal(123, result.Code);
        //    Assert.Equal("Province A", result.Name);
        //}
        //[Fact]
        //public async Task UpdateAsync_ThrowsBusinessException_WhenCodeAlreadyExists()
        //{
        //    // Arrange
        //    var input = new CreateUpdateProvinceDto { Code = 123, Name = "Province A" };
        //    var existingProvince = new Province { Code = 124, Name = "Province B" };
        //    _repositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Province, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(existingProvince);
        //    // Act & Assert
        //    var exception = await Assert.ThrowsAsync<BusinessException>(() => _service.UpdateAsync(1, input));
        //    Assert.Equal("Mã: 123 đã tồn tại trong hệ thống", exception.Message);
        //}

        //[Fact]
        //public async Task UpdateAsync_UpdatesProvinceSuccessfully_WhenCodeDoesNotExist()
        //{
        //    // Arrange
        //    var input = new CreateUpdateProvinceDto { Code = 123, Name = "Province A" };
        //    var existingProvince = new Province { Code = 124, Name = "Province B" };

        //    _repositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Province, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync((Province)null); // No existing province with this code

        //    _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Province>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns((Task<Province>)Task.CompletedTask);

        //    // Act
        //    var result = await _service.UpdateAsync(1, input);

        //    // Assert
        //    _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Province>(), It.IsAny<bool>(),It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.Equal(123, result.Code);
        //    Assert.Equal("Province A", result.Name);

        //}
        //[Fact]
        //public async Task DeleteAsync_DeletesProvinceSuccessfully()
        //{
        //    // Arrange
        //    var provinceId = 1;
        //    _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        //        .Returns(Task.CompletedTask);

        //    // Act
        //    await _service.DeleteAsync(provinceId);

        //    // Assert
        //    _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        //}

        //[Fact]
        //public async Task ImportExcel_ThrowsBusinessException_WhenFileHasInvalidExtension()
        //{
        //    // Arrange
        //    var fileMock = new Mock<IFormFile>();
        //    fileMock.Setup(f => f.FileName).Returns("invalid.txt");

        //    // Act & Assert
        //    var exception = await Assert.ThrowsAsync<BusinessException>(() => _service.ImportExcel(fileMock.Object, true));
        //    Assert.Equal("File phải có định dạng xlsx", exception.Message);
        //}

        //[Fact]
        //public async Task ImportExcel_ImportsDataSuccessfully_WhenFileIsValid()
        //{
        //    // Arrange
        //    var fileMock = new Mock<IFormFile>();
        //    fileMock.Setup(f => f.FileName).Returns("provinces.xlsx");
        //    var fileStream = new MemoryStream();
        //    fileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);

        //    _excelServiceMock.Setup(s => s.ImportExcelFileAsync<Province>(It.IsAny<Stream>(), It.IsAny<bool>(), It.IsAny<Func<ExcelWorksheet, int, Province>>(), It.IsAny<Func<int, Task<Province>>>(), It.IsAny<Func<IEnumerable<Province>, Task>>(), It.IsAny<Func<IEnumerable<Province>, Task>>(), It.IsAny<Func<Province, ExcelWorksheet, int, Province>>()))
        //        .Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _service.ImportExcel(fileMock.Object, true);

        //    // Assert
        //    Assert.True(result);
        //    _excelServiceMock.Verify(s => s.ImportExcelFileAsync<Province>(It.IsAny<Stream>(), It.IsAny<bool>(), It.IsAny<Func<ExcelWorksheet, int, Province>>(), It.IsAny<Func<int, Task<Province>>>(), It.IsAny<Func<IEnumerable<Province>, Task>>(), It.IsAny<Func<IEnumerable<Province>, Task>>(), It.IsAny<Func<Province, ExcelWorksheet, int, Province>>()), Times.Once);
        //}
    }
}

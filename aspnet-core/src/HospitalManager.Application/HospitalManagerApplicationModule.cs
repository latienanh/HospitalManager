using System;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FluentValidation;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace HospitalManager;

[DependsOn(
    typeof(HospitalManagerDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(HospitalManagerApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpFluentValidationModule)
    )]
public class HospitalManagerApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var licenseContext = configuration["EPPlus:ExcelPackage:LicenseContext"];

        if (Enum.TryParse(licenseContext, out OfficeOpenXml.LicenseContext contextValue))
        {
            ExcelPackage.LicenseContext = contextValue;
        }
        else
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;  // Default to Commercial license
        }
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<HospitalManagerApplicationModule>();
        });
    }
}

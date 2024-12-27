using System;
using HospitalManager.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OfficeOpenXml;
using Quartz;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Smtp;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FluentValidation;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Quartz;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
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
    typeof(AbpFluentValidationModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpQuartzModule),
    typeof(AbpEmailingModule)
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
        Configure<AbpSettingOptions>(options =>
        {
            options.DefinitionProviders.Add<EmailingSettingDefinitionProvider>();
        });

        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = true;
        });
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, SmtpEmailSender>());


        context.Services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = new JobKey("DailyReportJob");

            q.AddJob<DailyReportJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("DailyReportJob-trigger")
                .WithCronSchedule("0 57 14 * * ?"));
        });

        context.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    }
}

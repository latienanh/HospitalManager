using System.Collections.Generic;
using System.Text;
using HospitalManager.Entities;
using Quartz;
using System.Threading.Tasks;
using HospitalManager.Dtos.Response;
using PatientManager.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.Uow;

public class DailyReportJob : IJob, ITransientDependency
{
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly PatientService _patientService;


    public DailyReportJob(IEmailSender emailSender, IUnitOfWorkManager unitOfWorkManager, PatientService patientService)
    {
        _emailSender = emailSender;
        _unitOfWorkManager = unitOfWorkManager;
        _patientService = patientService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            // Truy vấn thông tin
            var newPatientsByHospital = await _patientService.HospitalPatientCountReport();
            var newPatientsByProvince = await _patientService.ProvincePatientCountReport();

            var hospitalReportHtml = GenerateHospitalPatientReportHtml(newPatientsByHospital); 
            var provinceReportHtml = GenerateProvincePatientReportHtml(newPatientsByProvince);

            var emailBody = new StringBuilder();
            emailBody.Append("<h1>Báo cáo hàng ngày</h1>"); 
            emailBody.Append(hospitalReportHtml); 
            emailBody.Append(provinceReportHtml);
            // Gửi email
            await _emailSender.SendAsync(
                "latienanh328@gmail.com",
                "Báo cáo hàng ngày",
                emailBody.ToString(),
                isBodyHtml: true
            );

            await uow.CompleteAsync();
        }
    }
    private string GenerateHospitalPatientReportHtml(List<HospitalPatientCountDto> report)
    {
        var sb = new StringBuilder();

        sb.Append("<h2>Báo cáo bệnh nhân theo bệnh viện</h2>");
        sb.Append("<table border='1' style='width:100%; border-collapse:collapse;'>");
        sb.Append("<tr><th>Bệnh viện</th><th>Số lượng</th></tr>");

        foreach (var item in report)
        {
            sb.Append($"<tr><td>{item.HospitalId}</td><td>{item.Count}</td></tr>");
        }

        sb.Append("</table>");

        return sb.ToString();
    }

    private string GenerateProvincePatientReportHtml(List<ProvincePatientCountDto> report)
    {
        var sb = new StringBuilder();

        sb.Append("<h2>Báo cáo bệnh nhân theo tỉnh thành</h2>");
        sb.Append("<table border='1' style='width:100%; border-collapse:collapse;'>");
        sb.Append("<tr><th>Tỉnh thành</th><th>Số lượng</th></tr>");

        foreach (var item in report)
        {
            sb.Append($"<tr><td>{item.ProvinceCode}</td><td>{item.Count}</td></tr>");
        }

        sb.Append("</table>");

        return sb.ToString();
    }

}
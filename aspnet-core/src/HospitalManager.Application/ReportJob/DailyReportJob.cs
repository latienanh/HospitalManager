using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using HospitalManager.Entities;
using Quartz;
using System.Threading.Tasks;
using HospitalManager.Abstractions;
using HospitalManager.Dtos.Response;
using PatientManager.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.Uow;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class DailyReportJob(
    IEmailSender emailSender,
    IUnitOfWorkManager unitOfWorkManager,
    PatientService patientService,
    IUserDapperRepository userDapperRepository
    )
    : IJob, ITransientDependency
{
    public async Task Execute(IJobExecutionContext context)
    {
        using (var uow = unitOfWorkManager.Begin())
        {
            // Truy vấn thông tin
            var newPatientsByHospital = await patientService.HospitalPatientCountReport();
            var newPatientsByProvince = await patientService.ProvincePatientCountReport();

            var hospitalReportHtml = GenerateHospitalPatientReportHtml(newPatientsByHospital); 
            var provinceReportHtml = GenerateProvincePatientReportHtml(newPatientsByProvince);

            var emailBody = new StringBuilder();
            var dateNow = DateTime.Now;
            emailBody.Append("<h1>Báo cáo hàng ngày</h1>");
            emailBody.Append($"<h3>Thời gian tạo : {dateNow.ToString("yyyy-MM-dd HH:mm:ss")}</h3>");
            emailBody.Append(hospitalReportHtml); 
            emailBody.Append(provinceReportHtml);

            var emailsTo = await userDapperRepository.GetGmailByRole("admin");

            foreach (var email in emailsTo)
            {
                 await emailSender.SendAsync(
                email,
                "Báo cáo hàng ngày",
                emailBody.ToString(),
                isBodyHtml: true
                );
            }
           

            await uow.CompleteAsync();
        }
    }
    private string GenerateHospitalPatientReportHtml(IEnumerable<HospitalPatientCountDto> report)
    {
        var sb = new StringBuilder();
        

        // Chuyển đổi đối tượng Date thành chuỗi có định dạng ngày giờ
        sb.Append("<h2>Báo cáo bệnh nhân theo bệnh viện</h2>");
        sb.Append("<table border='1' style='width:100%; border-collapse:collapse;'>");
        sb.Append("<tr><th>HospitalId</th><th>Mã bệnh viện</th> <th>Tên Bệnh viện</th> <th>Số lượng bệnh nhân</th></tr>");

        foreach (var item in report)
        {
            sb.Append($"<tr><td>{item.HospitalId}</td><td>{item.Code}</td><td>{item.Name}</td><td>{item.CountPatient}</td></tr>");
        }

        sb.Append("</table>");

        return sb.ToString();
    }

    private string GenerateProvincePatientReportHtml(IEnumerable<ProvincePatientCountDto> report)
    {
        var sb = new StringBuilder();

        sb.Append("<h2>Báo cáo bệnh nhân theo tỉnh thành</h2>");
        sb.Append("<table border='1' style='width:100%; border-collapse:collapse;'>");
        sb.Append("<tr><th>Mã tỉnh thành</th><th>Tên Tỉnh thành</th><th>Số lượng bệnh nhân</th></tr>");

        foreach (var item in report)
        {
            sb.Append($"<tr><td>{item.ProvinceCode}</td><td>{item.Name}</td><td>{item.CountPatient}</td></tr>");
        }

        sb.Append("</table>");

        return sb.ToString();
    }

}
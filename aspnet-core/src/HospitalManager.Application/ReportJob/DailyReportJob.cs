using HospitalManager.Entities;
using Quartz;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.Uow;

public class DailyReportJob : IJob, ITransientDependency
{
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public DailyReportJob(IEmailSender emailSender, IUnitOfWorkManager unitOfWorkManager, IRepository<Patient, int> repository)
    {
        _emailSender = emailSender;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            // Truy vấn thông tin
            int newPatientsByHospital = GetNewPatientsByHospital();
            int newPatientsByProvince = GetNewPatientsByProvince();

            // Gửi email
            await _emailSender.SendAsync(
                "latienanh328@gmail.com",
                "Báo cáo hàng ngày",
                $"Số bệnh nhân mới nhập viện trong ngày: {newPatientsByHospital}\n" +
                $"Số bệnh nhân mới thuộc các tỉnh trong ngày: {newPatientsByProvince}"
            );

            await uow.CompleteAsync();
        }
    }

    private int GetNewPatientsByHospital()
    {
        return 0;
    }

    private int GetNewPatientsByProvince()
    {
        // Thực hiện truy vấn để lấy số liệu bệnh nhân mới thuộc các tỉnh trong ngày
        return 0; // Thay bằng logic thực tế
    }
}
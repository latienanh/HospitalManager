using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Settings;

namespace HospitalManager.Setting
{
    public class EmailingSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition(
                EmailSettingNames.Smtp.Host,
                "smtp.gmail.com",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.Smtp.Port,
                "587",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.Smtp.UserName,
                "xinchaomoinguoi1231@gmail.com",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.Smtp.Password,
                "zkdg pyng vnvo cnhu",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.Smtp.EnableSsl,
                "true",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.Smtp.UseDefaultCredentials,
                "false",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.DefaultFromAddress,
                "xinchaomoinguoi1231@gmail.com",
                isVisibleToClients: false
            ));

            context.Add(new SettingDefinition(
                EmailSettingNames.DefaultFromDisplayName,
                "Report",
                isVisibleToClients: false
            ));

        }
    }
}

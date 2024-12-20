using Volo.Abp.Settings;

namespace HospitalManager.Settings;

public class HospitalManagerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(HospitalManagerSettings.MySetting1));
    }
}

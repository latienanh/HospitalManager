using HospitalManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HospitalManager.Permissions;

public class HospitalManagerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HospitalManagerPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(HospitalManagerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HospitalManagerResource>(name);
    }
}

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
        var provinceManager = myGroup.AddPermission("Province_Manager",
      LocalizableString.Create<HospitalManagerResource>("Permission:Province_Manager")
  );
        provinceManager.AddChild("Province_Create",
            LocalizableString.Create<HospitalManagerResource>("Permission:Province_Create")
        );
        provinceManager.AddChild("Province_Delete",
            LocalizableString.Create<HospitalManagerResource>("Permission:Province_Delete")
        );
        provinceManager.AddChild("Province_Update",
            LocalizableString.Create<HospitalManagerResource>("Permission:Province_Update")
        );
        provinceManager.AddChild("Province_Import",
            LocalizableString.Create<HospitalManagerResource>("Permission:Province_Import")
        );
        provinceManager.AddChild("Province_GetPaging",
            LocalizableString.Create<HospitalManagerResource>("Permission:Province_GetPaging")
        );

        var districtManager = myGroup.AddPermission("District_Manager",
            LocalizableString.Create<HospitalManagerResource>("Permission:District_Manager")
        );
        districtManager.AddChild("District_Create",
            LocalizableString.Create<HospitalManagerResource>("Permission:District_Create")
        );
        districtManager.AddChild("District_Delete",
            LocalizableString.Create<HospitalManagerResource>("Permission:District_Delete")
        );
        districtManager.AddChild("District_Update",
            LocalizableString.Create<HospitalManagerResource>("Permission:District_Update")
        );
        districtManager.AddChild("District_Import",
            LocalizableString.Create<HospitalManagerResource>("Permission:District_Import")
        );
        districtManager.AddChild("District_GetPaging",
            LocalizableString.Create<HospitalManagerResource>("Permission:District_GetPaging")
        );

        var wardManager = myGroup.AddPermission("Ward_Manager",
            LocalizableString.Create<HospitalManagerResource>("Permission:Ward_Manager")
        );
        wardManager.AddChild("Ward_Create",
            LocalizableString.Create<HospitalManagerResource>("Permission:Ward_Create")
        );
        wardManager.AddChild("Ward_Delete",
            LocalizableString.Create<HospitalManagerResource>("Permission:Ward_Delete")
        );
        wardManager.AddChild("Ward_Update",
            LocalizableString.Create<HospitalManagerResource>("Permission:Ward_Update")
        );
        wardManager.AddChild("Ward_Import",
            LocalizableString.Create<HospitalManagerResource>("Permission:Ward_Import")
        );
        wardManager.AddChild("Ward_GetPaging",
            LocalizableString.Create<HospitalManagerResource>("Permission:Ward_GetPaging")
        );

        var hospitalManager = myGroup.AddPermission("Hospital_Manager",
            LocalizableString.Create<HospitalManagerResource>("Permission:Hospital_Manager")
        );
        hospitalManager.AddChild("Hospital_Create",
            LocalizableString.Create<HospitalManagerResource>("Permission:Hospital_Create")
        );
        hospitalManager.AddChild("Hospital_Delete",
            LocalizableString.Create<HospitalManagerResource>("Permission:Hospital_Delete")
        );
        hospitalManager.AddChild("Hospital_Update",
            LocalizableString.Create<HospitalManagerResource>("Permission:Hospital_Update")
        );
        hospitalManager.AddChild("Hospital_GetUserNotInHospital",
            LocalizableString.Create<HospitalManagerResource>("Permission:Hospital_GetUserNotInHospital")
        );
        hospitalManager.AddChild("Hospital_GetPaging",
            LocalizableString.Create<HospitalManagerResource>("Permission:Hospital_GetPaging")
        );

        var patientManager = myGroup.AddPermission("Patient_Manager",
            LocalizableString.Create<HospitalManagerResource>("Permission:Patient_Manager")
        );
        patientManager.AddChild("Patient_Create",
            LocalizableString.Create<HospitalManagerResource>("Permission:Patient_Create")
        );
        patientManager.AddChild("Patient_Delete",
            LocalizableString.Create<HospitalManagerResource>("Permission:Patient_Delete")
        );
        patientManager.AddChild("Patient_Update",
            LocalizableString.Create<HospitalManagerResource>("Permission:Patient_Update")
        );
        patientManager.AddChild("Patient_GetPaging",
            LocalizableString.Create<HospitalManagerResource>("Permission:Patient_GetPaging")
        );

        var userHospitalManager = myGroup.AddPermission("UserHospital_Manager",
            LocalizableString.Create<HospitalManagerResource>("Permission:UserHospital_Manager")
        );
        userHospitalManager.AddChild("UserHospital_Delete",
            LocalizableString.Create<HospitalManagerResource>("Permission:UserHospital_Delete")
        );

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HospitalManagerResource>(name);
    }
}

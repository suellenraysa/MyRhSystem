namespace MyRhSystem.APP.Shared.Models;

public class CompanyRegisterModel
{
    public CompanyBasicInfoModel BasicInfo { get; set; } = new();
    public CompanyAddressModel Address { get; set; } = new();
    public LegalRepresentativeModel LegalRepresentative { get; set; } = new();
}
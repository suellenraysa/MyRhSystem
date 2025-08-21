using MyRhSystem.Contracts.Common;

namespace MyRhSystem.Contracts.Companies;

public sealed class CreateCompanyRequest
{
    public string? Nome { get; set; }
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    
    public int? AddressId { get; set; }
    public int? RepresentativeId { get; set; }

    public AddressCreateRequest? Address { get; set; }
    public RepresentativeCreateRequest? Representative { get; set; }
}



namespace MyRhSystem.Contracts.Companies;

public sealed class CompanyDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? AddressId { get; set; }
    public int? RepresentativeId { get; set; }
    public DateTime CreatedAt { get; set; }
    


    public int EmployeesCount { get; set; }
}

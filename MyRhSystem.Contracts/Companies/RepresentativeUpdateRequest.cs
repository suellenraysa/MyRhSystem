namespace MyRhSystem.Contracts.Companies;

public sealed class RepresentativeUpdateRequest
{
    public string FullName { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
}
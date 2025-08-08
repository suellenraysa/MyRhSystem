namespace MyRhSystem.Application.Employees;

using System.ComponentModel.DataAnnotations;

public class EmployeeDto
{
    public Guid Id { get; set; }

    [Required, StringLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Sobrenome { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Cargo { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Departamento { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Telefone { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string CEP { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Endereco { get; set; } = string.Empty;
    [Required, StringLength(30)]
    public string Bairro { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Cidade { get; set; } = string.Empty;

    [Required, StringLength(2, MinimumLength = 2)]
    public string UF { get; set; } = "SP";

    [DataType(DataType.Date)]
    public DateTime? Contratacao { get; set; } = DateTime.Today;

    [Range(0, 1_000_000)]
    public decimal Salario { get; set; }

    public bool Ativo { get; set; } = true;
}
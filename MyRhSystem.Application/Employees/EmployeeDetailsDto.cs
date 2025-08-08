using MyRhSystem.Application.Employees;

public class EmployeeDetailsDto : EmployeeDto
{
    // Pessoais
    public DateTime? DataNascimento { get; set; }
    public string? Sexo { get; set; }
    public string? EstadoCivil { get; set; }

    // Documentos
    public string? Cpf { get; set; }
    public string? Rg { get; set; }
    public string? RgOrgaoEmissor { get; set; }
    public string? RgUf { get; set; }

    public string? CtpsNumero { get; set; }
    public string? CtpsSerie { get; set; }
    public string? CtpsUf { get; set; }

    public string? TituloEleitorNumero { get; set; }
    public string? TituloEleitorZona { get; set; }
    public string? TituloEleitorSecao { get; set; }

    // Endereço
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public new string? Bairro { get; set; }

    // Contrato
    public string? RegimeTrabalho { get; set; }
    public string? Horario { get; set; }         // 8h-17h etc.

    // Benefícios
    public bool BeneficioVr { get; set; }
    public bool BeneficioVa { get; set; }
    public bool BeneficioVt { get; set; }
    public bool BeneficioPlanoSaude { get; set; }
    public string? OutrosBeneficios { get; set; }

    // Contato de emergência
    public string? EmergenciaNome { get; set; }
    public string? EmergenciaParentesco { get; set; }
    public string? EmergenciaTelefone { get; set; }

    // Dependentes
    public List<DependenteDto> Dependentes { get; set; } = new();
}

public class DependenteDto
{
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public DateTime? Nascimento { get; set; }
    public string? GrauParentesco { get; set; }
    public bool ParaIr { get; set; }
    public bool ParaSalarioFamilia { get; set; }
}

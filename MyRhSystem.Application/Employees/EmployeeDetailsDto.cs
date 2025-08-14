using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Application.Employees
{
    /// <summary>
    /// Detalhes completos do funcionário para telas de cadastro/edição/visualização.
    /// Herda os campos básicos de <see cref="EmployeeDto"/>.
    /// </summary>
    public class EmployeeDetailsDto : EmployeeDto
    {
        #region Documentos

        [MaxLength(14)]
        [RegularExpression(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", ErrorMessage = "CPF inválido.")]
        public string? Cpf { get; set; }

        [MaxLength(20)] public string? Rg { get; set; }

        [MaxLength(10)] public string? OrgaoEmissor { get; set; }

        [DataType(DataType.Date)] public DateTime? RgEmissao { get; set; }

        [MaxLength(20)] public string? CtpsNumero { get; set; }

        [MaxLength(20)] public string? CtpsSerie { get; set; }

        [MaxLength(20)] public string? TituloEleitor { get; set; }

        [MaxLength(20)] public string? Zona { get; set; }

        [MaxLength(20)] public string? Sessao { get; set; }

        #endregion

        #region Filiação e Escolaridade

        public string? NomeMae { get; set; }
        public string? NomePai { get; set; }
        public string? GrauInstrucao { get; set; }

        #endregion

        #region Contrato

        public string? Funcao { get; set; }
        public string? TipoContrato { get; set; }
        public TimeOnly? HorarioEntrada { get; set; }
        public TimeOnly? HorarioSaida { get; set; }
        public int? JornadaHoras { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Admissao { get; set; }
        public DateTime? DataExperiencia { get; set; }
        public DateTime? DataExperiencia1 { get; set; }

        public IEnumerable<string> Beneficios { get; set; } = Array.Empty<string>();

        #endregion

        #region Dependentes

        public List<DependenteDto> Dependentes { get; set; } = new();

        #endregion

        #region Contatos

        public List<ContatoDto> Contatos { get; set; } = new();

        #endregion

        // 🔹 Fábrica: cria um Details a partir de um EmployeeDto base
        public static EmployeeDetailsDto From(EmployeeDto e)
        {
            if (e is null) throw new ArgumentNullException(nameof(e));

            return new EmployeeDetailsDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Sobrenome = e.Sobrenome,
                Email = e.Email,
                Telefone = e.Telefone,
                Cargo = e.Cargo,
                Departamento = e.Departamento,
                Endereco = e.Endereco,
                Numero = e.Numero,
                Bairro = e.Bairro,
                Cep = e.Cep,
                Cidade = e.Cidade,
                UF = e.UF,
                Contratacao = e.Contratacao,
                Salario = e.Salario,
                Ativo = e.Ativo
            };
        }
    }

    /// <summary>Representa um dependente do funcionário.</summary>
    public class DependenteDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Informe o nome do dependente.")]
        public string Nome { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? Nascimento { get; set; }

        public string? GrauParentesco { get; set; }
    };
    /// <summary>Representa um contato de emergência do funcionário.</summary>
    public class ContatoDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Informe o nome do Contato.")]
        public string Nome { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Telefone { get; set; }

        public string? GrauParentesco { get; set; }
    }
}
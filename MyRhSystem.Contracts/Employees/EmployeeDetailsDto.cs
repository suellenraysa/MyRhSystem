using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Application.Employees
{
    public class EmployeeDetailsDto
    {
        public Guid Id { get; set; }

        // Pessoais
        [Required, StringLength(100)]
        public string Nome { get; set; } = "";

        [Required, StringLength(150)]
        public string Sobrenome { get; set; } = "";

        [Required, StringLength(10)]
        public string Sexo { get; set; } = "";

        [Required]
        public DateTime DataNascimento { get; set; }

        [StringLength(80), EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(30)]
        public string Telefone { get; set; } = "";

        // Profissional
        [Required, StringLength(255)]
        public string Cargo { get; set; } = "";

        [Required, StringLength(255)]
        public string Departamento { get; set; } = "";

        [Required, StringLength(255)]
        public string Funcao { get; set; } = "";

        [Required, StringLength(255)]
        public string GrauInstrucao { get; set; } = "";

        [Required]
        public bool Ativo { get; set; }

        // Filiação
        [StringLength(100)]
        public string? NomeMae { get; set; }

        [StringLength(100)]
        public string? NomePai { get; set; }

        // Documentos
        [Required, StringLength(14)] // CPF formatado: 000.000.000-00
        public string Cpf { get; set; } = "";

        [StringLength(20)]
        public string? Rg { get; set; }

        [StringLength(50)]
        public string? OrgaoEmissor { get; set; }

        public DateTime? RgEmissao { get; set; }

        [StringLength(20)]
        public string? TituloEleitor { get; set; }

        [StringLength(10)]
        public string? Zona { get; set; }

        [StringLength(10)]
        public string? Sessao { get; set; }

        [Required, StringLength(30)]
        public string CtpsNumero { get; set; } = "";

        [Required, StringLength(10)]
        public string CtpsSerie { get; set; } = "";

        // Contrato
        [Required, StringLength(20)]
        public string TipoContrato { get; set; } = "";   // valores via AppDefaults.WorkRegime

        public int? JornadaHoras { get; set; }

        [Required]
        public DateTime Admissao { get; set; }

        [Required]
        public DateTime DataExperiencia { get; set; }

        [Required]
        public DateTime DataExperiencia1 { get; set; }

        public DateTime Contratacao { get; set; }
        public decimal Salario { get; set; }

        // Endereço
        [StringLength(255)]
        public string? Endereco { get; set; }

        [StringLength(10)]
        public string? Numero { get; set; }

        [StringLength(100)]
        public string? Bairro { get; set; }

        [StringLength(100)]
        public string? Cidade { get; set; }

        [StringLength(2)]
        public string? UF { get; set; } // valores via AppDefaults.States

        [StringLength(15)]
        public string? Cep { get; set; }

        // Coleções
        public List<DependenteDto>? Dependentes { get; set; }
        public List<ContatoDto>? Contatos { get; set; }

        // Outros
        [Required]
        public string[] Beneficios { get; set; } = Array.Empty<string>();

        [Required]
        public TimeSpan HorarioEntrada { get; set; }

        [Required]
        public TimeSpan HorarioSaida { get; set; }
    }

    public class DependenteDto
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string? Nome { get; set; }

        public DateTime? Nascimento { get; set; }

        [StringLength(50)]
        public string? GrauParentesco { get; set; }
    }

    public class ContatoDto
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string? Nome { get; set; }

        [StringLength(50)]
        public string? GrauParentesco { get; set; }

        [StringLength(30)]
        public string? Telefone { get; set; }
    }
}

using MyRhSystem.Domain.Entities.Funcionarios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Payroll;

[Table("folha_de_pagamento")]
public class FolhaDePagamento
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("funcionario_id")]
    public int FuncionarioId { get; set; }

    [Column("hora_entrada")]
    public DateTime HoraEntrada { get; set; }

    [Column("hora_saida")]
    public DateTime HoraSaida { get; set; }

    [Column("salario_base")]
    public decimal SalarioBase { get; set; }

    [Column("bonus")]
    public decimal Bonus { get; set; }

    [Column("deducoes")]
    public decimal Deducoes { get; set; }

    [Column("salario_liquido")]
    public decimal SalarioLiquido { get; set; }

    [Column("data_pagamento")]
    public DateTime DataPagamento { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(FuncionarioId))]
    public Funcionario? Funcionario { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Employees
{
    /// <summary>
    /// Contrato do funcionário (1:1 com employees).
    /// </summary>
    [Table("employee_contracts")]
    public class EmployeeContract
    {
        // PK = FK -> employees.id
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        // ---- Dados contratuais ----
        [Required, StringLength(50)]
        [Column("tipo_contrato")]
        public string TipoContrato { get; set; } = "";

        [Column("jornada_horas")]
        public int? JornadaHoras { get; set; }

        [Required]
        [Column("admissao")]
        public DateTime Admissao { get; set; }

        [Column("experiencia_1")]
        public DateTime? Experiencia1 { get; set; }

        [Column("experiencia_2")]
        public DateTime? Experiencia2 { get; set; }

        [Column("salario", TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        // Horários (Entrada e Saída)
        [Column("hora_entrada")]
        public TimeSpan? HoraEntrada { get; set; }

        [Column("hora_saida")]
        public TimeSpan? HoraSaida { get; set; }

        // Auditoria
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}

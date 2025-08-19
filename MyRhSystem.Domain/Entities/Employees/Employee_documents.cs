using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyRhSystem.Domain.Entities.Employees
{
    /// <summary>
    /// Documentos do funcionário (1:1 com employees).
    /// PK = FK (employee_id) para garantir cardinalidade 1:1.
    /// </summary>
    [Table("employee_documents")]
    [Index(nameof(Cpf), IsUnique = true, Name = "IX_employee_documents_cpf_unique")]
    public class EmployeeDocuments
    {
        // PK = FK -> employees.id
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        // ---- Campos de documentos ----
        [Required, StringLength(14)]
        [Column("cpf")]
        public string Cpf { get; set; } = "";

        [StringLength(20)]
        [Column("rg")]
        public string? Rg { get; set; }

        [StringLength(50)]
        [Column("orgao_emissor")]
        public string? OrgaoEmissor { get; set; }

        [Column("rg_emissao")]
        public DateTime? RgEmissao { get; set; }

        [StringLength(20)]
        [Column("pis")]
        public string? Pis { get; set; }

        [StringLength(20)]
        [Column("titulo_eleitor")]
        public string? TituloEleitor { get; set; }

        [StringLength(10)]
        [Column("zona")]
        public string? Zona { get; set; }

        [StringLength(10)]
        [Column("sessao")]
        public string? Sessao { get; set; }

        [Required, StringLength(30)]
        [Column("ctps_numero")]
        public string CtpsNumero { get; set; } = "";

        [Required, StringLength(10)]
        [Column("ctps_serie")]
        public string CtpsSerie { get; set; } = "";

        // Auditoria
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}

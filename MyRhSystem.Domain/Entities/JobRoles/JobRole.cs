using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.JobRoles
{
    [Table("job_roles")]
    public class JobRole
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(150)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        // ========= FK: Department =========
        [Required]
        [ForeignKey(nameof(Department))]
        [Column("department_id")]
        public int DepartmentId { get; set; }
        public Departments.Department Department { get; set; } = null!;

        // ========= FK: JobLevel =========
        [Required]
        [ForeignKey(nameof(Level))]
        [Column("level_id")]
        public int LevelId { get; set; }
        public JobLevels Level  { get; set; } = null!;

        // ========= Campos do Cargo =========
        [Required]
        [Column("salario_base", TypeName = "decimal(18,2)")]
        public decimal SalarioBase { get; set; }

        [Column("salario_maximo", TypeName = "decimal(18,2)")]
        public decimal? SalarioMaximo { get; set; }

        [Column("requisitos")]
        public string? Requisitos { get; set; }

        [Column("responsabilidades")]
        public string? Responsabilidades { get; set; }

        // ========= Auditoria =========
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Employees
{
    [Table("employee_dependents")]
    public class EmployeeDependents
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("nascimento")]
        public DateTime? Nascimento { get; set; }

        [StringLength(50)]
        [Column("grau_parentesco")]
        public string? GrauParentesco { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Relacionamento com employees
        [ForeignKey(nameof(Employee))]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = default!;
    }
}

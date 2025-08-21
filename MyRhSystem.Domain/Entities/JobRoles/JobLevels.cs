using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyRhSystem.Domain.Entities.Companies;

namespace MyRhSystem.Domain.Entities.JobRoles
{
    [Table("job_levels")]
    public class JobLevels
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("company_id")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = default!;

        // ========= Auditoria =========
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<JobRole> JobRoles { get; set; } = new List<JobRole>();
    }
}

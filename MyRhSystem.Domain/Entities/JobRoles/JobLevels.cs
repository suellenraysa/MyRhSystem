using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        /// <summary>
        /// Ordem hierárquica do nível 
        /// (1 = Estagiário, 2 = Júnior, 3 = Pleno, 4 = Sênior, 5 = Trainer).
        /// </summary>
        [Required]
        [Column("ordem")]
        public int Ordem { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}

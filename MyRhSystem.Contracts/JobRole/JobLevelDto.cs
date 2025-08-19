using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Contracts.JobRole
{
    public class JobLevelDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Ordem hierárquica do nível (1 = Estagiário, 2 = Júnior, 3 = Pleno, etc).
        /// </summary>
        [Required]
        public int Ordem { get; set; }
    }
}

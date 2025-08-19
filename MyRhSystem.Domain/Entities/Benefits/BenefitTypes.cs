using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Benefits
{
    [Table("benefit_types")]
    public class BenefitTypes
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("descricao")]
        public string? Descricao { get; set; }

        [ForeignKey(nameof(Category))]
        [Column("category_id")]
        public int CategoryId { get; set; }
        public BenefitCategories Category { get; set; } = default!;
    }
}

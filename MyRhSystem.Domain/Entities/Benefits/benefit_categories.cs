using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Benefits
{
    [Table("benefit_categories")]
    public class BenefitCategories
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;
    }
}

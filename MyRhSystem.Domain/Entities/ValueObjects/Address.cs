using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyRhSystem.Domain.Entities.ValueObjects
{
    [Table("addresses")]
    [Index(nameof(Cep), Name = "IX_addresses_cep")]
    public class Address
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(255)]
        [Column("endereco")]
        public string Endereco { get; set; } = "";

        [StringLength(20)]
        [Column("numero")]
        public string? Numero { get; set; }

        [StringLength(120)]
        [Column("bairro")]
        public string? Bairro { get; set; }

        [StringLength(120)]
        [Column("cidade")]
        public string? Cidade { get; set; }

        [StringLength(2)]
        [Column("uf")]
        public string? UF { get; set; }

        [StringLength(15)]
        [Column("cep")]
        public string? Cep { get; set; }

        [StringLength(255)]
        [Column("complemento")]
        public string? Complemento { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}

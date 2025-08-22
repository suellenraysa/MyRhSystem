using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.APP.Shared.Models;

[Table("reminders")]
public class Reminder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // vínculo obrigatório com empresa
    [Column("company_id")]
    public int CompanyId { get; set; }
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = default!;

    // opcional: responsável/atribuição
    [Column("assigned_user_id")]
    public int? AssignedUserId { get; set; }
    [ForeignKey(nameof(AssignedUserId))]
    public User? AssignedUser { get; set; }

    [Column("title")]
    public string Title { get; set; } = default!;

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("due_at")]
    public DateTime? DueAt { get; set; } // prazo

    [Column("remind_at")]
    public DateTime? RemindAt { get; set; } // quando notificar

    [Column("is_done")]
    public bool IsDone { get; set; }

    // recorrência (opcional – pode começar simples e evoluir)
    [Column("recurrence")]
    public string? RecurrenceRRule { get; set; } // ex: "FREQ=MONTHLY;INTERVAL=1"

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}


namespace MyRhSystem.APP.Shared.Models;

public class ReminderModel
{
    public string Titulo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public string Prioridade { get; set; } = "Média"; // Alta, Média, Baixa
    public DateTime Data { get; set; }
    public bool Concluido { get; set; }
}


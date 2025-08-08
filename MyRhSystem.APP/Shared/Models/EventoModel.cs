namespace MyRhSystem.APP.Shared.Models;

public class EventoModel
{
    public string Titulo { get; set; } = "";
    public DateTime DataHora { get; set; }
    public string Cor { get; set; } = "blue"; // Classe CSS: blue, red, green...
}

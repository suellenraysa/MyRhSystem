namespace MyRhSystem.APP.Shared.Constants;

public static class AppDefaults
{
    public static readonly string[] Genders = new[]
    {
        "Masculino", "Feminino", "Outro", "Prefiro não informar"
    };

    public static readonly string[] MaritalStatus = new[]
    {
        "Solteiro(a)", "Casado(a)", "Divorciado(a)", "Viúvo(a)", "União Estável"
    };

    public static readonly string[] EducationLevels = new[]
    {
        "Ensino Fundamental Incompleto", "Ensino Fundamental Completo",
        "Ensino Médio Incompleto", "Ensino Médio Completo",
        "Ensino Superior Incompleto", "Ensino Superior Completo",
        "Pós-graduação", "Mestrado", "Doutorado"
    };

    public static readonly string[] IncomeRanges = new[]
    {
        "Até R$ 1.000", "R$ 1.001 - R$ 2.000", "R$ 2.001 - R$ 3.000",
        "R$ 3.001 - R$ 5.000", "R$ 5.001 - R$ 8.000", "R$ 8.001 - R$ 12.000",
        "R$ 12.001 - R$ 20.000", "Acima de R$ 20.000"
    };
}

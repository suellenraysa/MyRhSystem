namespace MyRhSystem.APP.Shared.Constantes
{
    public enum TempoMensagemUI
    {
        Curto,   // ~2s
        Medio,   // ~3s
        Longo    // ~5s
    }

    public static class UIMensagens
    {
        public static int ObterTempoEmMs(TempoMensagemUI tempo)
        {
            return tempo switch
            {
                TempoMensagemUI.Curto => 2000,
                TempoMensagemUI.Medio => 3000,
                TempoMensagemUI.Longo => 5000,
                _ => 3000
            };
        }
    }
}

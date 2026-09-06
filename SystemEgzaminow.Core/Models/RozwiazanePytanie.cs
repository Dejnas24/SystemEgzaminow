using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.Models
{
    public class RozwiazanePytanie
    {
        public int Id { get; set; }

        // Id pytania źródłowego zapisane tylko informacyjnie.
        // Bez relacji FK, ponieważ pytanie może zostać zmienione lub usunięte.
        public int? IdPytaniaZrodlowego { get; set; }

        // Migawka pytania
        public string TrescPytania { get; set; } = string.Empty;

        public TypPytaniaEnum TypPytania { get; set; }

        public int LiczbaPunktowMax { get; set; }
        public int LiczbaPunktowZdobytych { get; set; }

        // Id autora pytania do późniejszych raportów.
        // Celowo bez właściwości nawigacyjnej.
        public int IdAutoraPytania { get; set; }

        // Kolejność pytania widziana przez ucznia
        public int Kolejnosc { get; set; }

        // Relacja z całym wynikiem/podejściem ucznia
        public int IdWynikuTestu { get; set; }

        public WynikTestu WynikTestu { get; set; } = null!;

        public List<RozwiazanaOdpowiedz> RozwiazaneOdpowiedzi { get; set; } = new();
    }
}
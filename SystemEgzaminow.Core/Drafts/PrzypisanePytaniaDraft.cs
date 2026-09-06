using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.Drafts
{
    public class PrzypisanePytaniaDraft
    {
        public int PytanieId { get; set; }
        public string TrescPytania { get; set; } = string.Empty;
        public TypPytaniaEnum TypPytania { get; set; }
        public int LiczbaPunktow { get; set; }
        public string? SciezkaZdjecia { get; set; }
        public int Kolejnosc { get; set; }
    }
}
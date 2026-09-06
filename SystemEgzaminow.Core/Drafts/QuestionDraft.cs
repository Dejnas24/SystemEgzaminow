using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.Drafts
{
    public class QuestionDraft
    {
        public string TrescPytania { get; set; } = string.Empty;
        public TypPytaniaEnum TypPytania { get; set; }
        public int LiczbaPunktow { get; set; }
        public int IdAutora { get; set; }
        public string? SciezkaZdjecia { get; set; }

        public List<OdpowiedzDraft> Odpowiedzi { get; set; } = new();
    }
}
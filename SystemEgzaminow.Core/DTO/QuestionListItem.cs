using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class QuestionListItem
    {
        public int Id { get; set; }

        public string TrescPytania { get; set; } = string.Empty;

        public TypPytaniaEnum TypPytania { get; set; }

        public int LiczbaPunktow { get; set; }

        public int IdAutora { get; set; }

        public string AutorPelnaNazwa { get; set; } = string.Empty;

        public bool CzyZarchiwizowane { get; set; } = false;
        public string? SciezkaZdjecia { get; set; }

        public string TypPytaniaTekst =>
    TypPytania switch
    {
        TypPytaniaEnum.JednokrotnyWybor => "Jednokrotny wybór",
        TypPytaniaEnum.WielokrotnyWybor => "Wielokrotny wybór",
        TypPytaniaEnum.Otwarte => "Otwarte",
        _ => "Nieznany"
    };

        public string StatusPytania =>
            CzyZarchiwizowane ? "Zarchiwizowane" : "Aktywne";

        public string ArchiveButtonText =>
            CzyZarchiwizowane ? "Przywróć" : "Archiwizuj";

        public bool CzyMoznaEdytowac => !CzyZarchiwizowane;

        public bool CzyMoznaUsunac => CzyZarchiwizowane;
    }
}
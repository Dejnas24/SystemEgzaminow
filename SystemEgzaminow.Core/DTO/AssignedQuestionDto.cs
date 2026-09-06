using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class AssignedQuestionDto
    {
        public int Id { get; set; }

        public string TrescPytania { get; set; } = string.Empty;

        public TypPytaniaEnum TypPytania { get; set; }

        public int LiczbaPunktow { get; set; }

        public string AutorPelnaNazwa { get; set; } = string.Empty;

        public string? SciezkaZdjecia { get; set; }

        public List<AssignedAnswerDto> Odpowiedzi { get; set; } = new();

        public string OdpowiedzWzorcowa { get; set; } = string.Empty;

        public int NumerPytania { get; set; }

        public int LiczbaWszystkichPytan { get; set; }

        public bool CzyMoznaPrzesunacWGore { get; set; }

        public bool CzyMoznaPrzesunacWDol { get; set; }

        public bool CzyJednokrotnyWybor =>
            TypPytania == TypPytaniaEnum.JednokrotnyWybor;

        public bool CzyWielokrotnyWybor =>
            TypPytania == TypPytaniaEnum.WielokrotnyWybor;

        public bool CzyOtwarte =>
            TypPytania == TypPytaniaEnum.Otwarte;

        public bool CzyMaZdjecie =>
            !string.IsNullOrWhiteSpace(SciezkaZdjecia);

        public string TypPytaniaTekst => TypPytania switch
        {
            TypPytaniaEnum.JednokrotnyWybor =>
                "Jednokrotny wybór",

            TypPytaniaEnum.WielokrotnyWybor =>
                "Wielokrotny wybór",

            TypPytaniaEnum.Otwarte =>
                "Pytanie otwarte",

            _ => "Nieznany typ"
        };
    }
}
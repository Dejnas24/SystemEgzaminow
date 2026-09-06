using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class ResultQuestionDto
    {
        public string TrescPytania { get; set; } = string.Empty;
        public TypPytaniaEnum TypPytania { get; set; }

        public int LiczbaPunktowMax { get; set; }
        public int LiczbaPunktowZdobytych { get; set; }

        public int Kolejnosc { get; set; }

        public List<ResultAnswerDto> Odpowiedzi { get; set; } = new();
    }
}
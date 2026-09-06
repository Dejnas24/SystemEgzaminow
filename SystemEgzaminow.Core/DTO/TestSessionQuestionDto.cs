using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class TestSessionQuestionDto
    {
        public int PytanieId { get; set; }

        public string TrescPytania { get; set; } = string.Empty;

        public TypPytaniaEnum TypPytania { get; set; }

        public int LiczbaPunktow { get; set; }

        public string? TrescOdpowiedziUcznia { get; set; }

        public List<TestSessionAnswerDto> Odpowiedzi { get; set; } = new();
    }
}
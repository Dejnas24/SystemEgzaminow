using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.Core.DTO
{
    public class TestSessionDto
    {
        public readonly SposobWyswietlaniaWynikuEnum SposobWyswietlaniaWyniku;

        public int TestId { get; set; }
        public int? PrzypisanyTestId { get; set; }
        public bool CzyProbny { get; set; }

        public string Tytul { get; set; } = string.Empty;

        public int CzasTrwaniaMinuty { get; set; }

        public bool LosujKolejnoscPytan { get; set; }

        public bool LosujKolejnoscOdpowiedzi { get; set; }
        public int UserId { get; set; }
        public DateTime DataRozpoczecia { get; set; }

        public List<TestSessionQuestionDto> Pytania { get; set; } = new();
    }
}
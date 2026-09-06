namespace SystemEgzaminow.Core.DTO
{
    public class CreateTestDto
    {
        public string Tytul { get; set; } = string.Empty;

        public string? Opis { get; set; }

        public int TypTestuId { get; set; }

        public int ProgZdania { get; set; }

        public int CzasTrwaniaMinuty { get; set; }

        public bool LosujKolejnoscPytan { get; set; }

        public bool LosujKolejnoscOdpowiedzi { get; set; }

        public List<CreateTestQuestionDto> Pytania { get; set; } = new();
    }
}
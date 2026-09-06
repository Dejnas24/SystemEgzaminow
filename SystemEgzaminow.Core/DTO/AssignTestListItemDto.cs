namespace SystemEgzaminow.Core.DTO
{
    public class AssignTestListItemDto
    {
        public int TestId { get; set; }

        public string Tytul { get; set; } = string.Empty;

        public string TypTestu { get; set; } = string.Empty;

        public int LiczbaPytan { get; set; }

        public int CzasTrwaniaMinuty { get; set; }
    }
}
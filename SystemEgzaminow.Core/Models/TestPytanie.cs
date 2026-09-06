namespace SystemEgzaminow.Core.Models
{
    public class TestPytanie
    {
        public int Id { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; } = null!;

        public int PytanieId { get; set; }
        public Pytanie Pytanie { get; set; } = null!;

        public int NumerKolejnosci { get; set; }
    }
}
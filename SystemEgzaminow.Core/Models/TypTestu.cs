namespace SystemEgzaminow.Core.Models
{
    public class TypTestu
    {
        public int Id { get; set; }

        public string NazwaTypu { get; set; } = string.Empty;

        public List<Test> Testy { get; set; } = new();

        public List<SkalaOcen> SkaleOcen { get; set; } = new();
    }
}
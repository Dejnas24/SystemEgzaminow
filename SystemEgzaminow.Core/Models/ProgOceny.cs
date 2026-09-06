namespace SystemEgzaminow.Core.Models
{
    public class ProgOceny
    {
        public int Id { get; set; }
        public int IdSkaliOcen { get; set; }

        public decimal Ocena { get; set; }
        public decimal ProgOd { get; set; }
        public decimal ProgDo { get; set; }

        public SkalaOcen SkalaOcen { get; set; } = null!;
    }
}
namespace SystemEgzaminow.Core.DTO
{
    public class AssignedAnswerDto
    {
        public int Id { get; set; }

        public string TrescOdpowiedzi { get; set; } = string.Empty;

        public bool CzyPoprawna { get; set; }

        public int? LiczbaPunktow { get; set; }
    }
}
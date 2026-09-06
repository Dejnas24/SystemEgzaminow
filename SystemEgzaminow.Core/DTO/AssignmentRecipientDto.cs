namespace SystemEgzaminow.Core.DTO
{
    public class AssignmentRecipientDto
    {
        public int Id { get; set; }

        public string Nazwa { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string KlasaGrupa { get; set; } = string.Empty;
    }
}
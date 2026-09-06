namespace SystemEgzaminow.Core.Models
{
    public class Rola
    {
        public int Id { get; set; }

        public string Nazwa { get; set; }

        public List<Uzytkownik> Uzytkownicy { get; set; }
    }
}
namespace SystemEgzaminow.Core.Models
{
    public class RozwiazanaOdpowiedz
    {
        public int Id { get; set; }

        // Relacja do migawki pytania
        public int IdRozwiazanegoPytania { get; set; }

        public RozwiazanePytanie RozwiazanePytanie { get; set; } = null!;

        // Id odpowiedzi źródłowej zapisane informacyjnie.
        // Nie tworzymy relacji FK do tabeli Odpowiedz.
        public int? IdOdpowiedziZrodlowej { get; set; }

        // Migawka treści odpowiedzi
        public string TrescOdpowiedzi { get; set; } = string.Empty;

        // Czy odpowiedź była poprawna w chwili rozpoczęcia testu
        public bool CzyPoprawna { get; set; }

        // Dla pytań zamkniętych
        public bool CzyWybranaPrzezUcznia { get; set; }

        // Dla pytania otwartego
        public string? TrescOdpowiedziUcznia { get; set; }

        // Kolejność odpowiedzi widziana przez ucznia
        public int Kolejnosc { get; set; }

        // Punkty przypisane do tej odpowiedzi, jeżeli stosujemy
        // punktację częściową
        public decimal LiczbaPunktow { get; set; }

        public string? KomentarzNauczyciela { get; set; }
    }
}
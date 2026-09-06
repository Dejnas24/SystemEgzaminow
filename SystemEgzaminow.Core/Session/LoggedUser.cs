namespace SystemEgzaminow.Core.Session
{
    public static class LoggedUser
    {
        public static int Id { get; set; }

        public static string Login { get; set; } = string.Empty;

        public static string Imie { get; set; } = string.Empty;

        public static string Nazwisko { get; set; } = string.Empty;

        public static int RolaId { get; set; }

        public static int? LoginLogId { get; set; }
    }
}
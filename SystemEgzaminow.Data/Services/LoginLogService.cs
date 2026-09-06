using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class LoginLogService
    {
        private readonly AppDbContext _context;

        public LoginLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LogLogowania> AddLoginLogAsync(
            int? uzytkownikId,
            bool czySukces,
            string? adresIp = null)
        {
            var log = new LogLogowania
            {
                UzytkownikId = uzytkownikId,
                DataLogowania = DateTime.Now,
                DataWylogowania = null,
                CzySukces = czySukces,
                Sesja = czySukces
                    ? SesjaEnum.Zalogowany
                    : SesjaEnum.NieudanaProba,
                AdresIp = adresIp ?? GetLocalIpAddress()
            };

            _context.LogiLogowan.Add(log);

            await _context.SaveChangesAsync();

            return log;
        }

        public async Task EndLoginSessionAsync(int logId)
        {
            var log = await _context.LogiLogowan
                .FirstOrDefaultAsync(l => l.Id == logId);

            if (log == null)
                return;

            log.DataWylogowania = DateTime.Now;
            log.Sesja = SesjaEnum.Wylogowany;

            await _context.SaveChangesAsync();
        }

        private static string? GetLocalIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                return host.AddressList
                    .FirstOrDefault(ip =>
                        ip.AddressFamily == AddressFamily.InterNetwork)?
                    .ToString();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<LoginLogDto>> GetLoginLogsAsync()
        {
            return await _context.LogiLogowan
                .AsNoTracking()
                .Include(l => l.Uzytkownik)
                    .ThenInclude(u => u!.Rola)
                .OrderByDescending(l => l.DataLogowania)
                .Select(l => new LoginLogDto
                {
                    Id = l.Id,
                    UzytkownikId = l.UzytkownikId,

                    Login = l.Uzytkownik != null
                        ? l.Uzytkownik.Login
                        : "-",

                    ImieNazwisko = l.Uzytkownik != null
                        ? l.Uzytkownik.Imie + " " + l.Uzytkownik.Nazwisko
                        : "Nieznany",

                    Rola = l.Uzytkownik == null
    ? "-"
    : l.Uzytkownik.RolaId == 1
        ? "Administrator"
        : l.Uzytkownik.RolaId == 2
            ? "Nauczyciel"
            : l.Uzytkownik.RolaId == 3
                ? "Uczeń"
                : "Nieznana",

                    DataLogowania = l.DataLogowania,
                    DataWylogowania = l.DataWylogowania,

                    CzySukces = l.CzySukces,
                    Sesja = l.Sesja,
                    AdresIp = l.AdresIp
                })
                .ToListAsync();
        }
    }
}
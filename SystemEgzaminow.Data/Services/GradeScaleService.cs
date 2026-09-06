using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class GradeScaleService
    {
        private readonly AppDbContext _context;

        public GradeScaleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TypTestu>> GetTestTypesAsync()
        {
            return await _context.TypyTestow
                .AsNoTracking()
                .OrderBy(t => t.NazwaTypu)
                .ToListAsync();
        }

        public async Task<GradeScaleDto?> GetActiveScaleAsync(
    int idTypuTestu,
    int? idUzytkownika)
        {
            var scale = await _context.SkaleOcen
                .AsNoTracking()
                .Include(s => s.TypTestu)
                .Include(s => s.ProgiOcen)
                .Where(s =>
                    s.IdTypuTestu == idTypuTestu &&
                    s.CzyAktywna)
                .Where(s =>
                    s.IdUzytkownika == idUzytkownika ||
                    s.IdUzytkownika == null)
                .OrderByDescending(s =>
                    s.IdUzytkownika == idUzytkownika)
                .ThenByDescending(s => s.DataUtworzenia)
                .FirstOrDefaultAsync();

            if (scale == null)
                return null;

            return new GradeScaleDto
            {
                Id = scale.Id,
                IdTypuTestu = scale.IdTypuTestu,
                NazwaTypuTestu = scale.TypTestu.NazwaTypu,
                IdUzytkownika = scale.IdUzytkownika,
                Nazwa = scale.Nazwa,
                CzyAktywna = scale.CzyAktywna,

                Progi = scale.ProgiOcen
                    .OrderBy(p => p.ProgOd)
                    .Select(p => new GradeThresholdDto
                    {
                        Id = p.Id,
                        Ocena = p.Ocena,
                        ProgOd = p.ProgOd,
                        ProgDo = p.ProgDo
                    })
                    .ToList()
            };
        }

        public async Task<GradeScaleDto?> CreateIndividualScaleAsync(int idTypuTestu, int idUzytkownika)
        {
            var defaultScale = await _context.SkaleOcen
                .AsNoTracking()
                .Include(s => s.TypTestu)
                .Include(s => s.ProgiOcen)
                .Where(s =>
                    s.IdTypuTestu == idTypuTestu &&
                    s.IdUzytkownika == null &&
                    s.CzyAktywna)
                .FirstOrDefaultAsync();

            if (defaultScale == null)
                return null;

            var newScale = new SkalaOcen
            {
                IdTypuTestu = idTypuTestu,
                IdUzytkownika = idUzytkownika,
                Nazwa = $"Moja skala - {defaultScale.TypTestu.NazwaTypu}",
                CzyAktywna = true,
                DataUtworzenia = DateTime.Now,

                ProgiOcen = defaultScale.ProgiOcen
                    .OrderBy(p => p.ProgOd)
                    .Select(p => new ProgOceny
                    {
                        Ocena = p.Ocena,
                        ProgOd = p.ProgOd,
                        ProgDo = p.ProgDo
                    })
                    .ToList()
            };

            _context.SkaleOcen.Add(newScale);
            await _context.SaveChangesAsync();

            return await GetActiveScaleAsync(
                idTypuTestu,
                idUzytkownika);
        }

        public async Task<bool> UpdateIndividualScaleAsync(GradeScaleDto dto, int idUzytkownika)
        {
            if (!dto.Id.HasValue)
                return false;

            var scale = await _context.SkaleOcen
                .Include(s => s.ProgiOcen)
                .FirstOrDefaultAsync(s =>
                    s.Id == dto.Id.Value &&
                    s.IdUzytkownika == idUzytkownika &&
                    s.CzyAktywna);

            if (scale == null)
                return false;

            // Dodatkowe zabezpieczenie:
            // skali globalnej nigdy nie modyfikujemy.
            if (scale.IdUzytkownika == null)
                return false;

            scale.Nazwa = dto.Nazwa;

            // Usuwamy stare progi indywidualnej skali
            _context.ProgiOcen.RemoveRange(scale.ProgiOcen);

            // Dodajemy aktualne progi z widoku
            scale.ProgiOcen = dto.Progi
                .Select(p => new ProgOceny
                {
                    Ocena = p.Ocena,
                    ProgOd = p.ProgOd,
                    ProgDo = p.ProgDo
                })
                .ToList();

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreDefaultScaleAsync(int idTypuTestu, int idUzytkownika)
        {
            var userScale = await _context.SkaleOcen
                .FirstOrDefaultAsync(s =>
                    s.IdTypuTestu == idTypuTestu &&
                    s.IdUzytkownika == idUzytkownika &&
                    s.CzyAktywna);

            if (userScale == null)
                return false;

            userScale.CzyAktywna = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal?> GetGradeAsync(int idTypuTestu, int? idUzytkownika, decimal procent)
        {
            var scale = await _context.SkaleOcen
                .AsNoTracking()
                .Include(s => s.ProgiOcen)
                .Where(s =>
                    s.IdTypuTestu == idTypuTestu &&
                    s.CzyAktywna)
                .Where(s =>
                    s.IdUzytkownika == idUzytkownika ||
                    s.IdUzytkownika == null)
                .OrderByDescending(s =>
                    s.IdUzytkownika == idUzytkownika)
                .ThenByDescending(s => s.DataUtworzenia)
                .FirstOrDefaultAsync();

            if (scale == null)
                return null;

            var prog = scale.ProgiOcen
                .FirstOrDefault(p =>
                    procent >= p.ProgOd &&
                    procent <= p.ProgDo);

            return prog?.Ocena;
        }
    }
}
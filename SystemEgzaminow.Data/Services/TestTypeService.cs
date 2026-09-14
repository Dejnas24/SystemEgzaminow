using Microsoft.EntityFrameworkCore;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Data.Context;

namespace SystemEgzaminow.Data.Services
{
    public class TestTypeService
    {
        private readonly AppDbContext _context;

        public TestTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TestTypeDto>> GetAllAsync()
        {
            return await _context.TypyTestow
                .AsNoTracking()
                .OrderBy(t => t.NazwaTypu)
                .Select(t => new TestTypeDto
                {
                    Id = t.Id,
                    NazwaTypu = t.NazwaTypu
                })
                .ToListAsync();
        }

        public async Task<List<TestTypeDto>> GetByClassAsync(int classId)
        {

          var typ =  _context.PrzypisaneTesty
                .AsNoTracking()
                .Where(pt => pt.KlasaId == classId)
                .Select(pt => pt.Test.TypTestu)
                .Distinct();


            return await typ
                .OrderBy(t => t.NazwaTypu)
                .Select(t => new TestTypeDto
                {
                    Id = t.Id,
                    NazwaTypu = t.NazwaTypu,
                   
                    
                })
                .ToListAsync();
        }



    }
}
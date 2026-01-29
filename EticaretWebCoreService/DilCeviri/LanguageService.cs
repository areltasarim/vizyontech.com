using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EticaretWebCoreEntity;
using Microsoft.EntityFrameworkCore;

namespace EticaretWebCoreService
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;

        public LanguageService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Diller> GetLanguages()
        {
            return _context.Diller.ToList();
        }

        public Diller GetLanguageByCulture(string culture)
        {
            return _context.Diller.FirstOrDefault(x => 
                x.DilKodlari.DilKodu.Trim().ToLower() == culture.Trim().ToLower());
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EticaretWebCoreEntity;
using Microsoft.EntityFrameworkCore;

namespace EticaretWebCoreService
{
    public class LocalizationService : ILocalizationService
    {
        private readonly AppDbContext _context;

        public LocalizationService(AppDbContext context)
        {
            _context = context;
        }

        public DilCeviriTranslate GetStringResource(string resourceKey, int languageId)
        {
            var model = _context.DilCeviriTranslate.FirstOrDefault(x =>
                    x.DilCeviri.Anahtar.Trim().ToLower() == resourceKey.Trim().ToLower()
                    && x.DilId == languageId);

            return model;
        }
    }
}

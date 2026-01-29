using EticaretWebCoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EticaretWebCoreService
{
    public interface ILanguageService
    {
        IEnumerable<Diller> GetLanguages();
        Diller GetLanguageByCulture(string culture);
    }
}

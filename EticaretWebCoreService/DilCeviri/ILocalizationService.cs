using EticaretWebCoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EticaretWebCoreService
{
    public interface ILocalizationService
    {
        DilCeviriTranslate GetStringResource(string resourceKey, int languageId);
    }
}

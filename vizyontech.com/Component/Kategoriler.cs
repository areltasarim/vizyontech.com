using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vizyontech.com.Component
{
    public class Kategoriler : ViewComponent
    {
        private UnitOfWork _uow = null;
        public Kategoriler()
        {
            _uow = new UnitOfWork();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _uow.Repository<EticaretWebCoreEntity.Kategoriler>().GetAll().Result.Where(x => x.Durum == SayfaDurumlari.Aktif && x.Id != 1).ToListAsync();
            return View(model);
        }

    }
}

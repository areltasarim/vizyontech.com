using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vizyontech.com.Component
{
    public class Markalar : ViewComponent
    {
        private UnitOfWork _uow = null;
        public Markalar()
        {
            _uow = new UnitOfWork();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _uow.Repository<EticaretWebCoreEntity.Markalar>().GetAll().Result.Where(x => x.Durum == SayfaDurumlari.Aktif).ToListAsync();
            return View(model);
        }

    }
}

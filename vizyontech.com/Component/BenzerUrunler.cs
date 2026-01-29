using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace vizyontech.com.Component
{
    public class BenzerUrunler : ViewComponent
    {
        private UnitOfWork _uow = null;

        public BenzerUrunler()
        {
            _uow = new UnitOfWork();

        }

        public async Task<IViewComponentResult> InvokeAsync(int kategoriId)
        {
            var urunToKategori =  _uow.Repository<UrunToKategori>().GetAll().Result.Where(x => x.KategoriId == kategoriId).Select(x => x.UrunId);
            var model = await _uow.Repository<Urunler>().GetAll().Result.Where(x => urunToKategori.Contains(x.Id)).ToListAsync();
            return View(model);
        }

    }
}

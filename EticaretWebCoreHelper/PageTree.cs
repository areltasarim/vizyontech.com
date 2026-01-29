using EticaretWebCoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public static class PageTree
    {
        public static IEnumerable<Sayfalar> ToPageTree(this Sayfalar Item)
        {
            List<Sayfalar> pages = new List<Sayfalar>();
            pages.Add(Item);
            if (Item.ParentSayfa.Id != 1)
                GetParent(Item, pages);
            pages.Reverse();
            return pages;
        }

        private static void GetParent(Sayfalar Item, List<Sayfalar> pages)
        {

            if (Item.ParentSayfa.Id != 1)
            {
                pages.Add(Item.ParentSayfa);
                GetParent(Item.ParentSayfa, pages);
            }
        }

        public static IEnumerable<Kategoriler> ToCategoryTree(this Kategoriler Item)
        {
            List<Kategoriler> pages = new List<Kategoriler>();
            pages.Add(Item);
            if (Item.ParentKategori.Id != 1)
                GetParent(Item, pages);
            pages.Reverse();
            return pages;
        }

        private static void GetParent(Kategoriler Item, List<Kategoriler> pages)
        {

            if (Item.ParentKategori.Id != 1)
            {
                pages.Add(Item.ParentKategori);
                GetParent(Item.ParentKategori, pages);
            }
        }
    }
}

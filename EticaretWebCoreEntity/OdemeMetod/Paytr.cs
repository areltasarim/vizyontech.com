using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EticaretWebCoreEntity
{
    public class Paytr : BaseEntity
    {
        public string MagazaNo { get; set; }
        public string MagazaParola { get; set; }
        public string MagazaAnahtar { get; set; }
        public string TestModu { get; set; }
        public TaksitSayilari TaksitSayisi { get; set; }
        public int MaksimumTaksitSayisi { get; set; }

        public int BasariliSiparisDurumId { get; set; }
        public virtual SiparisDurumlari BasariliOdemeDurumu { get; set; }

        public int HataliSiparisDurumId { get; set; }
        public virtual SiparisDurumlari HataliOdemeDurumu { get; set; }


        public int? DilId { get; set; }
        public virtual Diller Dil { get; set; }


        public int? ParaBirimId { get; set; }
        public virtual ParaBirimleri ParaBirimi { get; set; }


        public override void Build(ModelBuilder builder)
        {

            builder.Entity<Paytr>(entity =>
            {
                entity
                    .HasOne(p => p.BasariliOdemeDurumu)
                    .WithMany(p => p.PaytrBasariliOdemeDurumu)
                    .HasForeignKey(p => p.BasariliSiparisDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity
                    .HasOne(p => p.HataliOdemeDurumu)
                    .WithMany(p => p.PaytrHataliOdemeDurumu)
                    .HasForeignKey(p => p.HataliSiparisDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.Paytr)
                .WithOne(p => p.Dil)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<ParaBirimleri>(entity =>
            {
                entity
                .HasMany(p => p.Paytr)
                .WithOne(p => p.ParaBirimi)
                .HasForeignKey(p => p.ParaBirimId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }

}

using EticaretWebCoreEntity.Opak;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    public class OpakDbContext : DbContext
    {
        public OpakDbContext()
        {
        }

        public OpakDbContext(DbContextOptions<OpakDbContext> options) : base(options) { }

        public virtual DbSet<TBLCARISB> TBLCARISB { get; set; }
        public virtual DbSet<TBLCARIHAR> TBLCARIHAR { get; set; }
        public virtual DbSet<TBLSTOKHAR> TBLSTOKHAR { get; set; }
        public virtual DbSet<TBLSTOKSB> TBLSTOKSB { get; set; }
        public virtual DbSet<TBLBANKAHAR> TBLBANKAHAR { get; set; }
        public virtual DbSet<TBLBANKAKARTHAR> TBLBANKAKARTHAR { get; set; }
        public virtual DbSet<TBLPLASIYERSB> TBLPLASIYERSB { get; set; }
        public virtual DbSet<TBLSIPARIS> TBLSIPARIS { get; set; }
        public virtual DbSet<TBLSIPARISKALEM> TBLSIPARISKALEM { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

    }
}

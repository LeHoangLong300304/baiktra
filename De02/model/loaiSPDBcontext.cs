using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace De02.model
{
    public partial class loaiSPDBcontext : DbContext
    {
        public loaiSPDBcontext()
            : base("name=loaiSPDBcontext")
        {
        }

        public virtual DbSet<LoaiSP> LoaiSP { get; set; }
        public virtual DbSet<Sanpham> Sanphams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}

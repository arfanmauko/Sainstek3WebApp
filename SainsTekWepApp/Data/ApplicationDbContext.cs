using Microsoft.AspNet.Identity.EntityFramework;
using SainsTekWepApp.Entities;
using SainsTekWepApp.Migrations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SainsTekWepApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UserImage> UserImage { get; set; }

        public DbSet<Pengumuman> Pengumuman { get; set; }

        public DbSet<Page> Page { get; set; }

        public DbSet<Makalah> Makalah { get; set; }

        public DbSet<KategoriMakalah> KategoriMakalah { get; set; }

        public DbSet<PesanPublikToAdmin> PesanPublikToAdmin { get; set; }

        public DbSet<PesanUserToAdmin> PesanUserToAdmin { get; set; }

        public DbSet<PesanAdminToUser> PesanAdminToUser { get; set; }

        public DbSet<PembayaranPeserta> PembayaranPeserta { get; set; }

        public DbSet<PembayaranMakalah> PembayaranMakalah { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            base.OnModelCreating(modelBuilder);

            //UserImage
            modelBuilder.Entity<UserImage>()
                .HasKey(ui => ui.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(au => au.UserImage)
                .WithRequired(ui => ui.ApplicationUser);


            //Makalah
            modelBuilder.Entity<Makalah>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Makalah>()
                .Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Makalah>()
                .HasRequired(m => m.KategoriMakalah)
                .WithMany(km => km.ListMakalah)
                .HasForeignKey(m => m.CategoryId);

            modelBuilder.Entity<Makalah>()
                .HasRequired(m => m.ApplicationUser)
                .WithMany(a => a.ListMakalah)
                .HasForeignKey(m => m.userId);

            //PembayaranPeserta
            modelBuilder.Entity<PembayaranPeserta>()
                .HasKey(pp => pp.Id);
            modelBuilder.Entity<PembayaranPeserta>()
                .Property(pp => pp.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PembayaranPeserta>()
                .HasRequired(pp => pp.ApplicationUser)
                .WithMany(au => au.ListPembayaranPeserta)
                .HasForeignKey(pp => pp.UserId);

            //PembayaranMakalah
            modelBuilder.Entity<PembayaranMakalah>()
                .HasKey(pm => pm.Id);
            modelBuilder.Entity<PembayaranMakalah>()
                .Property(pm => pm.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PembayaranMakalah>()
                .HasRequired(pm => pm.Makalah)
                .WithMany(m => m.ListPembayaranMakalah)
                .HasForeignKey(pm => pm.MakalahId);


            //Pengumuman
            modelBuilder.Entity<Pengumuman>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Pengumuman>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Pengumuman>()
                .HasRequired(p => p.ApplicationUser)
                .WithMany(au => au.ListPengumuman)
                .HasForeignKey(p => p.UserId);

            //Page
            modelBuilder.Entity<Page>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Page>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Page>()
                .HasRequired(p => p.ApplicationUser)
                .WithMany(au => au.ListPage)
                .HasForeignKey(p => p.UserId);

            //PesanPublicToAdmin
            modelBuilder.Entity<PesanPublikToAdmin>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PesanPublikToAdmin>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PesanPublikToAdmin>()
                .HasOptional(p => p.ApplicationUser)
                .WithMany(au => au.ListPesanPublikToAdmin)
                .HasForeignKey(p => p.ReplyAdminId);

            //PesanUserToAdmin
            modelBuilder.Entity<PesanUserToAdmin>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PesanUserToAdmin>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PesanUserToAdmin>()
                .HasRequired(p => p.ApplicationUserByUserId)
                .WithMany(au => au.ListPesanUserToAdminByUserId)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<PesanUserToAdmin>()
                .HasOptional(p => p.ApplicationUserByAdminReaderId)
                .WithMany(au => au.ListPesanUserToAdminByAdminReaderId)
                .HasForeignKey(p => p.AdminReaderId);

            //PesanAdminToUser
            modelBuilder.Entity<PesanAdminToUser>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PesanAdminToUser>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PesanAdminToUser>()
                .HasOptional(p => p.ApplicationUserByAdminId)
                .WithMany(au => au.ListPesanAdminToUserByAdminId)
                .HasForeignKey(p => p.AdminId);
            modelBuilder.Entity<PesanAdminToUser>()
                .HasRequired(p => p.ApplicationUserByUserId)
                .WithMany(au => au.ListPesanAdminToUserByUserId)
                .HasForeignKey(p => p.UserId);
        }
    }
}
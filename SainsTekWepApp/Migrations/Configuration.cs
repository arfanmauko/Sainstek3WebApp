namespace SainsTekWepApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SainsTekWepApp.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SainsTekWepApp.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SainsTekWepApp.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Peserta"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Peserta" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Pemakalah"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Pemakalah" };

                manager.Create(role);
            }

            if (!context.Users.Any())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@sasa.com" };

                manager.Create(user, "sasasa");
                manager.AddToRole(user.Id, "Admin");
            }

            if (!context.KategoriMakalah.Any())
            {
                KategoriMakalah newKategoriMakalah1 = new KategoriMakalah
                {
                    Name = "Teknik Sipil, Material dan Infrastruktur",
                    Description = "Teknik Sipil, Material dan Infrastruktur"
                };

                KategoriMakalah newKategoriMakalah2 = new KategoriMakalah
                    {
                        Name = "Arsitektur dan Perencanaan wilayah",
                        Description = "Arsitektur dan Perencanaan wilayah"
                    };

                KategoriMakalah newKategoriMakalah3 = new KategoriMakalah
                   {
                       Name = "Ilmu Komputer, Informatika dan Teknik Elektro",
                       Description = "Ilmu Komputer, Informatika dan Teknik Elektro"
                   };

                KategoriMakalah newKategoriMakalah4 = new KategoriMakalah
                  {
                      Name = "Pertanian, Peternakan dan Manajemen Peternakan",
                      Description = "Pertanian, Peternakan dan Manajemen Peternakan"
                  };

                KategoriMakalah newKategoriMakalah5 = new KategoriMakalah
                  {
                      Name = "Matematika, Kimia, Fisika, Geofisika dan Astronomi",
                      Description = "Matematika, Kimia, Fisika, Geofisika dan Astronomi"
                  };

                KategoriMakalah newKategoriMakalah6 = new KategoriMakalah
                  {
                      Name = "Geologi, Tambang, Metalurgi, dan Teknik Mesin",
                      Description = "Geologi, Tambang, Metalurgi, dan Teknik Mesin"
                  };

                KategoriMakalah newKategoriMakalah7 = new KategoriMakalah
                  {
                      Name = "Kemaritiman dan Kelautan",
                      Description = "Kemaritiman dan Kelautan"
                  }; 

                KategoriMakalah newKategoriMakalah8 = new KategoriMakalah
                 {
                     Name = "Bidang Umum Lainnya",
                     Description = "Bidang Umum Lainnya"
                 };

                context.KategoriMakalah.Add(newKategoriMakalah1);
                context.KategoriMakalah.Add(newKategoriMakalah2);
                context.KategoriMakalah.Add(newKategoriMakalah3);
                context.KategoriMakalah.Add(newKategoriMakalah4);
                context.KategoriMakalah.Add(newKategoriMakalah5);
                context.KategoriMakalah.Add(newKategoriMakalah6);
                context.KategoriMakalah.Add(newKategoriMakalah7);
                context.KategoriMakalah.Add(newKategoriMakalah8);
            }

            int publicMessageCount = 1000;

            if (context.PesanPublikToAdmin.Count() < publicMessageCount)
            {
                List<PesanPublikToAdmin> listPesanPublic = new List<PesanPublikToAdmin>();

                for (int i = 0; i < publicMessageCount; i++)
                {
                    PesanPublikToAdmin publicMessage = new PesanPublikToAdmin
                    {
                        SenderEmail = string.Format("email{0}@hewqewq{0}.com", i + 1),
                        SenderName = string.Format("username{0}", i + 1),
                        MessageContent = string.Format("Message Content - {0}", i + 1),
                    };

                    if (i < publicMessageCount / 3)
                    {
                        publicMessage.DateAdminRead = DateTime.UtcNow.AddMinutes(i + 1);
                    }
                    else if (i < (publicMessageCount / 3) * 2)
                    {
                        publicMessage.DateAdminRead = DateTime.UtcNow.AddMinutes(i + 1);
                        publicMessage.DateReply = DateTime.UtcNow.AddMinutes(i + 5);
                    }

                    listPesanPublic.Add(publicMessage);
                }

                context.PesanPublikToAdmin.AddRange(listPesanPublic);
            }
        }
    }
}

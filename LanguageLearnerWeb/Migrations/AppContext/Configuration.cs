namespace LanguageLearnerWeb.Migrations.AppContext
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;

    internal sealed class Configuration : DbMigrationsConfiguration<LanguageLearnerWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\AppContext";
        }

        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
            {
                return HostingEnvironment.MapPath(seedFile);
            }
            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }

        private void SeedLevels(ApplicationDbContext context)
        {
            var levelsContents = System.IO.File.ReadAllText(MapPath(@"~/App_Data/Levels.json"));
            var levels = JsonConvert.DeserializeObject<List<Level>>(levelsContents).ToArray();
            context.Levels.AddOrUpdate(
                l => l.PointsRequired, levels);
        }

        private void SeedLanguages(ApplicationDbContext context)
        {
            context.Languages.AddOrUpdate(
                p => p.Id,
                new Language { Id = 1, Name = "English", NameOriginal = "English",
                    ShortName = "EN", ShortNameCC = "EN-US" },
                new Language { Id = 2, Name = "English", NameOriginal = "English",
                    ShortName = "EN", ShortNameCC = "EN-GB" },
                new Language { Id = 3, Name = "Ukrainian", NameOriginal = "Українська",
                    ShortName = "UK", ShortNameCC = "UK-UA"});
        }

        private void SeedMaterials(ApplicationDbContext context)
        {
            context.Materials.AddOrUpdate(
                p => p.Id,
                new Material { Id = 1, Difficulty = 1, Headline = "Hello", Text = "World", LanguageId = 1 },
                new Material { Id = 2, Difficulty = 1, Headline = "Привіт", Text = "Світ", LanguageId = 3 });
        }

        private void SeedPrepositions(ApplicationDbContext context)
        {
            context.Prepositions.AddOrUpdate(
                p => p.Id,
                new Preposition { Id = 1, LanguageId = 1, Prefix = "a", Suffix = "b", Options = "c", Answer = "d" },
                new Preposition { Id = 2, LanguageId = 2, Prefix = "q", Suffix = "w", Options = "e", Answer = "r" });
        }

        private void SeedWords(ApplicationDbContext context)
        {
            context.Words.AddOrUpdate(
                w => w.Id,
                new Word { Id = 1, Name = "a", Translation = "b", LanguageFromId = 1, LanguageToId = 2 },
                new Word { Id = 2, Name = "s", Translation = "d", LanguageFromId = 2, LanguageToId = 3 },
                new Word { Id = 3, Name = "q", Translation = "w", LanguageFromId = 3, LanguageToId = 1 });
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context))
                .Create(new IdentityRole("Admin"));
        }

        private void SeedProfiles(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            for (int i = 0; i < users.Length; ++i)
            {
                var user = new ApplicationUser {
                    Id = users[i], Email = users[i], UserName = users[i], };

                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    userManager.Create(user, "password123");
                    userManager.AddToRole(user.Id, "Admin");
                } else
                {
                    user.Id = context.Users.Find(user.UserName).Id;
                    userManager.Update(user);
                }

                context.Profiles.AddOrUpdate(
                    p => p.UserName,
                    new Profile { UserId = users[i], UserName = users[i], InterfaceLanguageId = 1 });
            }
        }

        private void SeedProfileLanguages(ApplicationDbContext context)
        {
            context.ProfileLanguages.AddOrUpdate(
                p => p.Id,
                new ProfileLanguage { Id = 1, LanguageId = 1, ProfileId = users[0], LevelId = 10 },
                new ProfileLanguage { Id = 2, LanguageId = 3, ProfileId = users[1], LevelId = 10 });
        }

        private void SeedProfileMaterials(ApplicationDbContext context)
        {
            context.ProfileMaterials.AddOrUpdate(
                m => m.Id,
                new ProfileMaterial { Id = 1, MaterialId = 1, ProfileId = users[0] },
                new ProfileMaterial { Id = 2, MaterialId = 2, ProfileId = users[1] });
        }

        private void SeedSettings(ApplicationDbContext context)
        {
            context.Settings.AddOrUpdate(
                s => s.Id,
                new Settings { Id = 1, ProfileId = users[0], Key = "1", Value = "1" },
                new Settings { Id = 1, ProfileId = users[0], Key = "2", Value = "2" },
                new Settings { Id = 1, ProfileId = users[1], Key = "1", Value = "3" },
                new Settings { Id = 1, ProfileId = users[1], Key = "2", Value = "4" });
        }

        private void SeedProfilePrepositions(ApplicationDbContext context)
        {
            context.ProfilePrepositions.AddOrUpdate(
                p => p.Id,
                new ProfilePreposition { Id = 1, PrepositionId = 1, ProfileId = users[0] },
                new ProfilePreposition { Id = 2, PrepositionId = 1, ProfileId = users[1] },
                new ProfilePreposition { Id = 3, PrepositionId = 2, ProfileId = users[1] });
        }

        private void SeedProfileWords(ApplicationDbContext context)
        {
            context.ProfileWords.AddOrUpdate(
                w => w.Id,
                new ProfileWord { Id = 1, ProfileId = users[0], WordId = 1 },
                new ProfileWord { Id = 2, ProfileId = users[0], WordId = 1 },
                new ProfileWord { Id = 3, ProfileId = users[0], WordId = 2 },
                new ProfileWord { Id = 4, ProfileId = users[1], WordId = 3 });
        }

        private string[] users =
        {
            "hello1@gmail.com",
            "hello2@gmail.com"
        };

        protected override void Seed(LanguageLearnerWeb.Models.ApplicationDbContext context)
        {
            SeedLevels(context);
            SeedLanguages(context);
            SeedMaterials(context);
            SeedPrepositions(context);
            SeedWords(context);

            SeedRoles(context);
            SeedProfiles(context);

            SeedProfileLanguages(context);
            SeedProfileMaterials(context);
            SeedSettings(context);
            SeedProfilePrepositions(context);
            SeedProfileWords(context);

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
        }
    }
}

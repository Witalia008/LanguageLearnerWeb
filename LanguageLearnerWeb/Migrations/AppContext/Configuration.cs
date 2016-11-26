namespace LanguageLearnerWeb.Migrations.AppContext
{
    using Microsoft.AspNet.Identity;
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

        private void SeedProfiles(ApplicationDbContext context)
        {
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("password123");
            string userName = "hello1@gmail.com";
            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser { Id = userName, UserName = userName, PasswordHash = password });

            context.Profiles.AddOrUpdate(
                p => p.UserName,
                new Profile { UserId = userName, UserName = userName, InterfaceLanguageId = 1 });

            password = passwordHash.HashPassword("password123");
            userName = "hello2@gmail.com";
            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser {Id = userName, UserName = userName, PasswordHash = password });

            context.Profiles.AddOrUpdate(
                p => p.UserName,
                new Profile { UserId = userName, UserName = userName, InterfaceLanguageId = 1 });
        }

        private void SeedProfileLanguages(ApplicationDbContext context)
        {
            context.ProfileLanguages.AddOrUpdate(
                p => p.Id,
                new ProfileLanguage { Id = 1, LanguageId = 1, ProfileId = "hello1@gmail.com", LevelId = 10 },
                new ProfileLanguage { Id = 2, LanguageId = 3, ProfileId = "hello1@gmail.com", LevelId = 10 });
        }

        private void SeedProfileMaterials(ApplicationDbContext context)
        {
            context.ProfileMaterials.AddOrUpdate(
                m => m.Id,
                new ProfileMaterial { Id = 1, MaterialId = 1, ProfileId = "hello1@gmail.com" },
                new ProfileMaterial { Id = 2, MaterialId = 2, ProfileId = "hello2@gmail.com" });
        }

        private void SeedSettings(ApplicationDbContext context)
        {
            context.Settings.AddOrUpdate(
                s => s.Id,
                new Settings { Id = 1, ProfileId = "hello1@gmail.com", Key = "1", Value = "1" },
                new Settings { Id = 1, ProfileId = "hello1@gmail.com", Key = "2", Value = "2" },
                new Settings { Id = 1, ProfileId = "hello2@gmail.com", Key = "1", Value = "3" },
                new Settings { Id = 1, ProfileId = "hello1@gmail.com", Key = "2", Value = "4" });
        }

        protected override void Seed(LanguageLearnerWeb.Models.ApplicationDbContext context)
        {
            SeedLevels(context);
            SeedLanguages(context);
            SeedMaterials(context);

            SeedProfiles(context);
            SeedProfileLanguages(context);
            SeedProfileMaterials(context);
            SeedSettings(context);

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

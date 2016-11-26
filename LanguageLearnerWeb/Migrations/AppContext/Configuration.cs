namespace LanguageLearnerWeb.Migrations.AppContext
{
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
                new Material { Id = 1, Difficulty = 1, Headline = "Привіт", Text = "Світ", LanguageId = 3 });
        }

        protected override void Seed(LanguageLearnerWeb.Models.ApplicationDbContext context)
        {
            SeedLevels(context);
            SeedLanguages(context);
            SeedMaterials(context);

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

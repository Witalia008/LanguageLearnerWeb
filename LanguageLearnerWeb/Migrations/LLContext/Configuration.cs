namespace LanguageLearnerWeb.Migrations.LLContext
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

    internal sealed class Configuration : DbMigrationsConfiguration<LanguageLearnerWeb.Models.LanguageLearnerWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\LLContext";
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

        protected override void Seed(LanguageLearnerWeb.Models.LanguageLearnerWebContext context)
        {
            var levelsContents = System.IO.File.ReadAllText(MapPath(@"~/App_Data/Levels.json"));
            var levels = JsonConvert.DeserializeObject<List<Level>>(levelsContents).ToArray();
            context.Levels.AddOrUpdate(
                l => l.PointsRequired, levels);
        }
    }
}

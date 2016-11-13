namespace LanguageLearnerWeb.Migrations.LLContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LanguageLearnerWeb.Models.LanguageLearnerWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\LLContext";
        }

        protected override void Seed(LanguageLearnerWeb.Models.LanguageLearnerWebContext context)
        {
        }
    }
}

namespace LanguageLearnerWeb.Migrations.LLContext
{
    using Models;
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
            context.Levels.AddOrUpdate(
                l => l.Id,
                new Level { Id = 1, PointsRequired = 10, Title = "Newbie" },
                new Level { Id = 2, PointsRequired = 20, Title = "Newbie" },
                new Level { Id = 3, PointsRequired = 40, Title = "Newbie" },
                new Level { Id = 4, PointsRequired = 80, Title = "Newbie" },
                new Level { Id = 5, PointsRequired = 160, Title = "Newbie" },
                new Level { Id = 6, PointsRequired = 500, Title = "Pupil" },
                new Level { Id = 7, PointsRequired = 750, Title = "Pupil" },
                new Level { Id = 8, PointsRequired = 1000, Title = "Pupil" },
                new Level { Id = 9, PointsRequired = 1250, Title = "Pupil" },
                new Level { Id = 10, PointsRequired = 1500, Title = "Pupil" },
                new Level { Id = 11, PointsRequired = 2000, Title = "Specialist" },
                new Level { Id = 12, PointsRequired = 2500, Title = "Specialist" },
                new Level { Id = 13, PointsRequired = 3000, Title = "Specialist" },
                new Level { Id = 14, PointsRequired = 3500, Title = "Specialist" },
                new Level { Id = 15, PointsRequired = 4000, Title = "Specialist" },
                new Level { Id = 16, PointsRequired = 5000, Title = "Expert" },
                new Level { Id = 17, PointsRequired = 6000, Title = "Expert" },
                new Level { Id = 18, PointsRequired = 7000, Title = "Expert" },
                new Level { Id = 19, PointsRequired = 8000, Title = "Expert" },
                new Level { Id = 20, PointsRequired = 9000, Title = "Expert" },
                new Level { Id = 21, PointsRequired = 10000, Title = "Candidate Master" },
                new Level { Id = 22, PointsRequired = 12000, Title = "Candidate Master" },
                new Level { Id = 23, PointsRequired = 14000, Title = "Candidate Master" },
                new Level { Id = 24, PointsRequired = 16000, Title = "Candidate Master" },
                new Level { Id = 25, PointsRequired = 18000, Title = "Candidate Master" },
                new Level { Id = 26, PointsRequired = 20000, Title = "Master" },
                new Level { Id = 27, PointsRequired = 23000, Title = "Master" },
                new Level { Id = 28, PointsRequired = 26000, Title = "Master" },
                new Level { Id = 29, PointsRequired = 29000, Title = "Master" },
                new Level { Id = 30, PointsRequired = 32000, Title = "Master" },
                new Level { Id = 31, PointsRequired = 35000, Title = "International Master" },
                new Level { Id = 32, PointsRequired = 40000, Title = "International Master" },
                new Level { Id = 33, PointsRequired = 45000, Title = "International Master" },
                new Level { Id = 34, PointsRequired = 50000, Title = "International Master" },
                new Level { Id = 35, PointsRequired = 55000, Title = "International Master" },
                new Level { Id = 36, PointsRequired = 60000, Title = "Grandmaster" },
                new Level { Id = 37, PointsRequired = 70000, Title = "Grandmaster" },
                new Level { Id = 38, PointsRequired = 80000, Title = "Grandmaster" },
                new Level { Id = 39, PointsRequired = 90000, Title = "Grandmaster" },
                new Level { Id = 40, PointsRequired = 100000, Title = "Grandmaster" },
                new Level { Id = 41, PointsRequired = 120000, Title = "International Grandmaster" },
                new Level { Id = 42, PointsRequired = 140000, Title = "International Grandmaster" },
                new Level { Id = 43, PointsRequired = 160000, Title = "International Grandmaster" },
                new Level { Id = 44, PointsRequired = 180000, Title = "International Grandmaster" },
                new Level { Id = 45, PointsRequired = 200000, Title = "International Grandmaster" },
                new Level { Id = 46, PointsRequired = 250000, Title = "Legendary Grandmaster" },
                new Level { Id = 47, PointsRequired = 300000, Title = "Legendary Grandmaster" },
                new Level { Id = 48, PointsRequired = 350000, Title = "Legendary Grandmaster" },
                new Level { Id = 49, PointsRequired = 400000, Title = "Legendary Grandmaster" },
                new Level { Id = 50, PointsRequired = 450000, Title = "Legendary Grandmaster" },
                new Level { Id = 51, PointsRequired = 999999999, Title = "God" }
                );
        }
    }
}

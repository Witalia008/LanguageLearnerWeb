namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileLanguages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        LevelId = c.Int(nullable: false),
                        Points = c.Int(defaultValue: 0),
                        DailyProgress = c.Double(defaultValue: 0),
                        IsOnLearn = c.Boolean(defaultValue: false),
                        IsActive = c.Boolean(defaultValue: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Levels", t => t.LevelId, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.LanguageId)
                .Index(t => t.ProfileId)
                .Index(t => t.LevelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileLanguages", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.ProfileLanguages", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.ProfileLanguages", "LanguageId", "dbo.Languages");
            DropIndex("dbo.ProfileLanguages", new[] { "LevelId" });
            DropIndex("dbo.ProfileLanguages", new[] { "ProfileId" });
            DropIndex("dbo.ProfileLanguages", new[] { "LanguageId" });
            DropTable("dbo.ProfileLanguages");
        }
    }
}

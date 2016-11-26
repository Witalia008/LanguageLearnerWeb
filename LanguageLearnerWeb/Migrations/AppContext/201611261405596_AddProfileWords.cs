namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileWords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileWords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordId = c.Int(nullable: false),
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        Progress = c.Int(defaultValue: 0),
                        IsLearning = c.Boolean(defaultValue: false),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .Index(t => t.WordId)
                .Index(t => t.ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileWords", "WordId", "dbo.Words");
            DropForeignKey("dbo.ProfileWords", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.ProfileWords", new[] { "ProfileId" });
            DropIndex("dbo.ProfileWords", new[] { "WordId" });
            DropTable("dbo.ProfileWords");
        }
    }
}

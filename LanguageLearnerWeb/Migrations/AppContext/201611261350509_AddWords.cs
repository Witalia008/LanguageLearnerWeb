namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageFromId = c.Int(nullable: false),
                        LanguageToId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Translation = c.String(nullable: false),
                        Transcription = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageFromId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageToId, cascadeDelete: true)
                .Index(t => t.LanguageFromId)
                .Index(t => t.LanguageToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Words", "LanguageToId", "dbo.Languages");
            DropForeignKey("dbo.Words", "LanguageFromId", "dbo.Languages");
            DropIndex("dbo.Words", new[] { "LanguageToId" });
            DropIndex("dbo.Words", new[] { "LanguageFromId" });
            DropTable("dbo.Words");
        }
    }
}

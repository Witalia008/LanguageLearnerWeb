namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrepositions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prepositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Prefix = c.String(nullable: false),
                        Suffix = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        Options = c.String(nullable: false),
                        Rule = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prepositions", "LanguageId", "dbo.Languages");
            DropIndex("dbo.Prepositions", new[] { "LanguageId" });
            DropTable("dbo.Prepositions");
        }
    }
}

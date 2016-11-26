namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaterials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Difficulty = c.Int(defaultValue: 0),
                        Image = c.String(),
                        Headline = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        ShortDescr = c.String(),
                        Rating = c.Double(defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Materials", "LanguageId", "dbo.Languages");
            DropIndex("dbo.Materials", new[] { "LanguageId" });
            DropTable("dbo.Materials");
        }
    }
}

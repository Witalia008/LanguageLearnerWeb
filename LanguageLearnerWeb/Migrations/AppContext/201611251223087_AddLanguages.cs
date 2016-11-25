namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLanguages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        Name = c.String(nullable: false),
                        NameOriginal = c.String(),
                        ShortName = c.String(nullable: false),
                        ShortNameCC = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ShortNameCC, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Languages", new[] { "ShortNameCC" });
            DropTable("dbo.Languages");
        }
    }
}

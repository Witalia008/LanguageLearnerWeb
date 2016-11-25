namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLevels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointsRequired = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Levels");
        }
    }
}

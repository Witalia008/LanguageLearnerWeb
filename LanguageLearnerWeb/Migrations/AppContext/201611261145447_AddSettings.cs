namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileId = c.String(maxLength: 128),
                        Key = c.String(nullable: false),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.ProfileId)
                .Index(t => t.ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Settings", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.Settings", new[] { "ProfileId" });
            DropTable("dbo.Settings");
        }
    }
}

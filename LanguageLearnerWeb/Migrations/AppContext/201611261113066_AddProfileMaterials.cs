namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileMaterials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaterialId = c.Int(nullable: false),
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        IsLearnt = c.Boolean(defaultValue: false),
                        Rating = c.Double(defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Materials", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.MaterialId)
                .Index(t => t.ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileMaterials", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.ProfileMaterials", "MaterialId", "dbo.Materials");
            DropIndex("dbo.ProfileMaterials", new[] { "ProfileId" });
            DropIndex("dbo.ProfileMaterials", new[] { "MaterialId" });
            DropTable("dbo.ProfileMaterials");
        }
    }
}

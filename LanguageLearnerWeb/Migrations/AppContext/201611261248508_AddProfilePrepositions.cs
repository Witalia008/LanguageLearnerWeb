namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfilePrepositions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfilePrepositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrepositionId = c.Int(nullable: false),
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        TimesSeen = c.Int(defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prepositions", t => t.PrepositionId, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.PrepositionId)
                .Index(t => t.ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfilePrepositions", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.ProfilePrepositions", "PrepositionId", "dbo.Prepositions");
            DropIndex("dbo.ProfilePrepositions", new[] { "ProfileId" });
            DropIndex("dbo.ProfilePrepositions", new[] { "PrepositionId" });
            DropTable("dbo.ProfilePrepositions");
        }
    }
}

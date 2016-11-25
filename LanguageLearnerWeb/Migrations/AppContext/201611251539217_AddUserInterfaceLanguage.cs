namespace LanguageLearnerWeb.Migrations.AppContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInterfaceLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "InterfaceLanguage_Id", c => c.Int());
            CreateIndex("dbo.Profiles", "InterfaceLanguage_Id");
            AddForeignKey("dbo.Profiles", "InterfaceLanguage_Id", "dbo.Languages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profiles", "InterfaceLanguage_Id", "dbo.Languages");
            DropIndex("dbo.Profiles", new[] { "InterfaceLanguage_Id" });
            DropColumn("dbo.Profiles", "InterfaceLanguage_Id");
        }
    }
}

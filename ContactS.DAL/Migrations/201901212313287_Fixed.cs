namespace ContactS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientProfiles", "Dialog_Id", "dbo.Dialogs");
            DropIndex("dbo.ClientProfiles", new[] { "Dialog_Id" });
            CreateTable(
                "dbo.DialogClientProfiles",
                c => new
                    {
                        Dialog_Id = c.Int(nullable: false),
                        ClientProfile_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Dialog_Id, t.ClientProfile_Id })
                .ForeignKey("dbo.Dialogs", t => t.Dialog_Id, cascadeDelete: true)
                .ForeignKey("dbo.ClientProfiles", t => t.ClientProfile_Id, cascadeDelete: true)
                .Index(t => t.Dialog_Id)
                .Index(t => t.ClientProfile_Id);
            
            DropColumn("dbo.ClientProfiles", "Dialog_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientProfiles", "Dialog_Id", c => c.Int());
            DropForeignKey("dbo.DialogClientProfiles", "ClientProfile_Id", "dbo.ClientProfiles");
            DropForeignKey("dbo.DialogClientProfiles", "Dialog_Id", "dbo.Dialogs");
            DropIndex("dbo.DialogClientProfiles", new[] { "ClientProfile_Id" });
            DropIndex("dbo.DialogClientProfiles", new[] { "Dialog_Id" });
            DropTable("dbo.DialogClientProfiles");
            CreateIndex("dbo.ClientProfiles", "Dialog_Id");
            AddForeignKey("dbo.ClientProfiles", "Dialog_Id", "dbo.Dialogs", "Id");
        }
    }
}

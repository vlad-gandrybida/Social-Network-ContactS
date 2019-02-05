namespace ContactS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Receiver_Id = c.String(nullable: false, maxLength: 128),
                        Sender_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientProfiles", t => t.Receiver_Id)
                .ForeignKey("dbo.ClientProfiles", t => t.Sender_Id, cascadeDelete: true)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "Sender_Id", "dbo.ClientProfiles");
            DropForeignKey("dbo.Requests", "Receiver_Id", "dbo.ClientProfiles");
            DropIndex("dbo.Requests", new[] { "Sender_Id" });
            DropIndex("dbo.Requests", new[] { "Receiver_Id" });
            DropTable("dbo.Requests");
        }
    }
}

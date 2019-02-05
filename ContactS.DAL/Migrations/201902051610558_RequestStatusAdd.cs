namespace ContactS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestStatusAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "IsConfirmed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Requests", "Status");
        }
    }
}

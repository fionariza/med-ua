namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedgender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MaleFemale", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MaleFemale");
        }
    }
}

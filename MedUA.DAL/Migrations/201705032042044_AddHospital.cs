namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHospital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hospitals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode:true),
                        Address = c.String(unicode: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "CurrentHospital_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "CurrentHospital_Id");
            AddForeignKey("dbo.AspNetUsers", "CurrentHospital_Id", "dbo.Hospitals", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CurrentHospital_Id", "dbo.Hospitals");
            DropIndex("dbo.AspNetUsers", new[] { "CurrentHospital_Id" });
            DropColumn("dbo.AspNetUsers", "CurrentHospital_Id");
            DropTable("dbo.Hospitals");
        }
    }
}

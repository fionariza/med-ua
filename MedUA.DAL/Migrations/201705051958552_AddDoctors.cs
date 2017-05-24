namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoctors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientUserDoctorUsers",
                c => new
                    {
                        PatientUser_Id = c.String(nullable: false, maxLength: 128),
                        DoctorUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PatientUser_Id, t.DoctorUser_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.PatientUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorUser_Id)
                .Index(t => t.PatientUser_Id)
                .Index(t => t.DoctorUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientUserDoctorUsers", "DoctorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PatientUserDoctorUsers", "PatientUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PatientUserDoctorUsers", new[] { "DoctorUser_Id" });
            DropIndex("dbo.PatientUserDoctorUsers", new[] { "PatientUser_Id" });
            DropTable("dbo.PatientUserDoctorUsers");
        }
    }
}

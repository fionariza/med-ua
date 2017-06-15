namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addhospitalresearchtoappointmet : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DoctorUserPatientUsers", newName: "PatientUserDoctorUsers");
            DropPrimaryKey("dbo.PatientUserDoctorUsers");
            AddPrimaryKey("dbo.PatientUserDoctorUsers", new[] { "PatientUser_Id", "DoctorUser_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PatientUserDoctorUsers");
            AddPrimaryKey("dbo.PatientUserDoctorUsers", new[] { "DoctorUser_Id", "PatientUser_Id" });
            RenameTable(name: "dbo.PatientUserDoctorUsers", newName: "DoctorUserPatientUsers");
        }
    }
}

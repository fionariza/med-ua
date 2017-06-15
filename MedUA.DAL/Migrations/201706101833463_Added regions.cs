namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedregions : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PatientUserDoctorUsers", newName: "DoctorUserPatientUsers");
            RenameTable(name: "dbo.ResearchEntries", newName: "EntryResearches");
            DropPrimaryKey("dbo.DoctorUserPatientUsers");
            DropPrimaryKey("dbo.EntryResearches");
            CreateTable(
                "dbo.PatientAppointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Appointment = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        HospitalResearch_Id = c.Int(),
                        PatientUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HospitalResearches", t => t.HospitalResearch_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientUser_Id)
                .Index(t => t.HospitalResearch_Id)
                .Index(t => t.PatientUser_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Oblast_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Oblasts", t => t.Oblast_Id)
                .Index(t => t.Oblast_Id);
            
            CreateTable(
                "dbo.Oblasts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HospitalResearches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Research_Id = c.Int(),
                        Hospital_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Researches", t => t.Research_Id)
                .ForeignKey("dbo.Hospitals", t => t.Hospital_Id)
                .Index(t => t.Research_Id)
                .Index(t => t.Hospital_Id);
            
            AddColumn("dbo.Hospitals", "SettlementName", c => c.String());
            AddColumn("dbo.Hospitals", "Region_Id", c => c.Int());
            AddPrimaryKey("dbo.DoctorUserPatientUsers", new[] { "DoctorUser_Id", "PatientUser_Id" });
            AddPrimaryKey("dbo.EntryResearches", new[] { "Entry_Id", "Research_Id" });
            CreateIndex("dbo.Hospitals", "Region_Id");
            AddForeignKey("dbo.Hospitals", "Region_Id", "dbo.Regions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientAppointments", "PatientUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.HospitalResearches", "Hospital_Id", "dbo.Hospitals");
            DropForeignKey("dbo.HospitalResearches", "Research_Id", "dbo.Researches");
            DropForeignKey("dbo.PatientAppointments", "HospitalResearch_Id", "dbo.HospitalResearches");
            DropForeignKey("dbo.Regions", "Oblast_Id", "dbo.Oblasts");
            DropForeignKey("dbo.Hospitals", "Region_Id", "dbo.Regions");
            DropIndex("dbo.HospitalResearches", new[] { "Hospital_Id" });
            DropIndex("dbo.HospitalResearches", new[] { "Research_Id" });
            DropIndex("dbo.Regions", new[] { "Oblast_Id" });
            DropIndex("dbo.Hospitals", new[] { "Region_Id" });
            DropIndex("dbo.PatientAppointments", new[] { "PatientUser_Id" });
            DropIndex("dbo.PatientAppointments", new[] { "HospitalResearch_Id" });
            DropPrimaryKey("dbo.EntryResearches");
            DropPrimaryKey("dbo.DoctorUserPatientUsers");
            DropColumn("dbo.Hospitals", "Region_Id");
            DropColumn("dbo.Hospitals", "SettlementName");
            DropTable("dbo.HospitalResearches");
            DropTable("dbo.Oblasts");
            DropTable("dbo.Regions");
            DropTable("dbo.PatientAppointments");
            AddPrimaryKey("dbo.EntryResearches", new[] { "Research_Id", "Entry_Id" });
            AddPrimaryKey("dbo.DoctorUserPatientUsers", new[] { "PatientUser_Id", "DoctorUser_Id" });
            RenameTable(name: "dbo.EntryResearches", newName: "ResearchEntries");
            RenameTable(name: "dbo.DoctorUserPatientUsers", newName: "PatientUserDoctorUsers");
        }
    }
}

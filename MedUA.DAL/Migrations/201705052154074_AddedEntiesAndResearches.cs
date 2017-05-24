namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEntiesAndResearches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Complains = c.String(unicode:true),
                        Examination = c.String(unicode: true),
                        Diagnosis = c.String(unicode: true),
                        Recomendations = c.String(unicode: true),
                        Doctor_Id = c.String(maxLength: 128),
                        Patient_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Doctor_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Patient_Id)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Patient_Id);
            
            CreateTable(
                "dbo.Researches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: true),
                        Entry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entries", t => t.Entry_Id)
                .Index(t => t.Entry_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Researches", "Entry_Id", "dbo.Entries");
            DropForeignKey("dbo.Entries", "Patient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Entries", "Doctor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Researches", new[] { "Entry_Id" });
            DropIndex("dbo.Entries", new[] { "Patient_Id" });
            DropIndex("dbo.Entries", new[] { "Doctor_Id" });
            DropTable("dbo.Researches");
            DropTable("dbo.Entries");
        }
    }
}

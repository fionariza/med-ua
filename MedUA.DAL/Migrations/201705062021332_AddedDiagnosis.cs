namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDiagnosis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "Difficulties", c => c.String());
            AddColumn("dbo.Entries", "AccordingDiagnosis", c => c.String());
            AddColumn("dbo.Entries", "QuestionDiagnosis", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Entries", "QuestionDiagnosis");
            DropColumn("dbo.Entries", "AccordingDiagnosis");
            DropColumn("dbo.Entries", "Difficulties");
        }
    }
}

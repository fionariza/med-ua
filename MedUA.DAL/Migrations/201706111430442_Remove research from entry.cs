namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removeresearchfromentry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EntryResearches", "Entry_Id", "dbo.Entries");
            DropForeignKey("dbo.EntryResearches", "Research_Id", "dbo.Researches");
            DropIndex("dbo.EntryResearches", new[] { "Entry_Id" });
            DropIndex("dbo.EntryResearches", new[] { "Research_Id" });
            DropTable("dbo.EntryResearches");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EntryResearches",
                c => new
                    {
                        Entry_Id = c.Int(nullable: false),
                        Research_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Entry_Id, t.Research_Id });
            
            CreateIndex("dbo.EntryResearches", "Research_Id");
            CreateIndex("dbo.EntryResearches", "Entry_Id");
            AddForeignKey("dbo.EntryResearches", "Research_Id", "dbo.Researches", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EntryResearches", "Entry_Id", "dbo.Entries", "Id", cascadeDelete: true);
        }
    }
}

namespace MedUA.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResearchmanytomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Researches", "Entry_Id", "dbo.Entries");
            DropIndex("dbo.Researches", new[] { "Entry_Id" });
            CreateTable(
                "dbo.ResearchEntries",
                c => new
                    {
                        Research_Id = c.Int(nullable: false),
                        Entry_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Research_Id, t.Entry_Id })
                .ForeignKey("dbo.Researches", t => t.Research_Id, cascadeDelete: true)
                .ForeignKey("dbo.Entries", t => t.Entry_Id, cascadeDelete: true)
                .Index(t => t.Research_Id)
                .Index(t => t.Entry_Id);
            
            DropColumn("dbo.Researches", "Entry_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Researches", "Entry_Id", c => c.Int());
            DropForeignKey("dbo.ResearchEntries", "Entry_Id", "dbo.Entries");
            DropForeignKey("dbo.ResearchEntries", "Research_Id", "dbo.Researches");
            DropIndex("dbo.ResearchEntries", new[] { "Entry_Id" });
            DropIndex("dbo.ResearchEntries", new[] { "Research_Id" });
            DropTable("dbo.ResearchEntries");
            CreateIndex("dbo.Researches", "Entry_Id");
            AddForeignKey("dbo.Researches", "Entry_Id", "dbo.Entries", "Id");
        }
    }
}

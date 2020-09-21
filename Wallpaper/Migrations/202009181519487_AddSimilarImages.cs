namespace Wallpaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSimilarImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Similars",
                c => new
                    {
                        Image1Id = c.Int(nullable: false),
                        Image2Id = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image1Id, t.Image2Id })
                .ForeignKey("dbo.Images", t => t.Image1Id)
                .ForeignKey("dbo.Images", t => t.Image2Id)
                .Index(t => t.Image1Id)
                .Index(t => t.Image2Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Similars", "Image2Id", "dbo.Images");
            DropForeignKey("dbo.Similars", "Image1Id", "dbo.Images");
            DropIndex("dbo.Similars", new[] { "Image2Id" });
            DropIndex("dbo.Similars", new[] { "Image1Id" });
            DropTable("dbo.Similars");
        }
    }
}

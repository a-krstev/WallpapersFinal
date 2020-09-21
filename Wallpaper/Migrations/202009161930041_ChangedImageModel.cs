namespace Wallpaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedImageModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "YearId", "dbo.Years");
            DropIndex("dbo.Images", new[] { "YearId" });
            AlterColumn("dbo.Images", "YearId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Images", "YearId");
            AddForeignKey("dbo.Images", "YearId", "dbo.Years", "Val", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "YearId", "dbo.Years");
            DropIndex("dbo.Images", new[] { "YearId" });
            AlterColumn("dbo.Images", "YearId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Images", "YearId");
            AddForeignKey("dbo.Images", "YearId", "dbo.Years", "Val");
        }
    }
}

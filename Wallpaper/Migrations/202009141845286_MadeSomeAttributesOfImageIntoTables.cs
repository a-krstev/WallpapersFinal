namespace Wallpaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeSomeAttributesOfImageIntoTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BodyTypes",
                c => new
                    {
                        Value = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Value);
            
            CreateTable(
                "dbo.CarBrands",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Value = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Value);
            
            CreateTable(
                "dbo.Years",
                c => new
                    {
                        Value = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Value);
            
            AddColumn("dbo.Images", "BodyType_Value", c => c.String(maxLength: 128));
            AddColumn("dbo.Images", "Brand_Name", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Images", "Color_Value", c => c.String(maxLength: 128));
            AddColumn("dbo.Images", "Year_Value", c => c.Int());
            CreateIndex("dbo.Images", "BodyType_Value");
            CreateIndex("dbo.Images", "Brand_Name");
            CreateIndex("dbo.Images", "Color_Value");
            CreateIndex("dbo.Images", "Year_Value");
            AddForeignKey("dbo.Images", "BodyType_Value", "dbo.BodyTypes", "Value");
            AddForeignKey("dbo.Images", "Brand_Name", "dbo.CarBrands", "Name", cascadeDelete: true);
            AddForeignKey("dbo.Images", "Color_Value", "dbo.Colors", "Value");
            AddForeignKey("dbo.Images", "Year_Value", "dbo.Years", "Value");
            DropColumn("dbo.Images", "Brand");
            DropColumn("dbo.Images", "BodyType");
            DropColumn("dbo.Images", "Color");
            DropColumn("dbo.Images", "Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Year", c => c.String());
            AddColumn("dbo.Images", "Color", c => c.String());
            AddColumn("dbo.Images", "BodyType", c => c.String());
            AddColumn("dbo.Images", "Brand", c => c.String(nullable: false));
            DropForeignKey("dbo.Images", "Year_Value", "dbo.Years");
            DropForeignKey("dbo.Images", "Color_Value", "dbo.Colors");
            DropForeignKey("dbo.Images", "Brand_Name", "dbo.CarBrands");
            DropForeignKey("dbo.Images", "BodyType_Value", "dbo.BodyTypes");
            DropIndex("dbo.Images", new[] { "Year_Value" });
            DropIndex("dbo.Images", new[] { "Color_Value" });
            DropIndex("dbo.Images", new[] { "Brand_Name" });
            DropIndex("dbo.Images", new[] { "BodyType_Value" });
            DropColumn("dbo.Images", "Year_Value");
            DropColumn("dbo.Images", "Color_Value");
            DropColumn("dbo.Images", "Brand_Name");
            DropColumn("dbo.Images", "BodyType_Value");
            DropTable("dbo.Years");
            DropTable("dbo.Colors");
            DropTable("dbo.CarBrands");
            DropTable("dbo.BodyTypes");
        }
    }
}

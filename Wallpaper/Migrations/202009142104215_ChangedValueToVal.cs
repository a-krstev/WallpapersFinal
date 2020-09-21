namespace Wallpaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedValueToVal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "BodyType_Value", "dbo.BodyTypes");
            DropForeignKey("dbo.Images", "Color_Value", "dbo.Colors");
            DropForeignKey("dbo.Images", "Year_Value", "dbo.Years");
            DropIndex("dbo.Images", new[] { "Year_Value" });
            RenameColumn(table: "dbo.Images", name: "BodyType_Value", newName: "BodyTypeId");
            RenameColumn(table: "dbo.Images", name: "Color_Value", newName: "ColorId");
            RenameColumn(table: "dbo.Images", name: "Year_Value", newName: "YearId");
            RenameColumn(table: "dbo.Images", name: "Brand_Name", newName: "BrandId");
            RenameIndex(table: "dbo.Images", name: "IX_Brand_Name", newName: "IX_BrandId");
            RenameIndex(table: "dbo.Images", name: "IX_BodyType_Value", newName: "IX_BodyTypeId");
            RenameIndex(table: "dbo.Images", name: "IX_Color_Value", newName: "IX_ColorId");
            DropPrimaryKey("dbo.BodyTypes");
            DropPrimaryKey("dbo.Colors");
            DropPrimaryKey("dbo.Years");
            AddColumn("dbo.BodyTypes", "Val", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Colors", "Val", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Years", "Val", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Images", "YearId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.BodyTypes", "Val");
            AddPrimaryKey("dbo.Colors", "Val");
            AddPrimaryKey("dbo.Years", "Val");
            CreateIndex("dbo.Images", "YearId");
            AddForeignKey("dbo.Images", "BodyTypeId", "dbo.BodyTypes", "Val");
            AddForeignKey("dbo.Images", "ColorId", "dbo.Colors", "Val");
            AddForeignKey("dbo.Images", "YearId", "dbo.Years", "Val");
            DropColumn("dbo.BodyTypes", "Value");
            DropColumn("dbo.Colors", "Value");
            DropColumn("dbo.Years", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Years", "Value", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Colors", "Value", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.BodyTypes", "Value", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Images", "YearId", "dbo.Years");
            DropForeignKey("dbo.Images", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.Images", "BodyTypeId", "dbo.BodyTypes");
            DropIndex("dbo.Images", new[] { "YearId" });
            DropPrimaryKey("dbo.Years");
            DropPrimaryKey("dbo.Colors");
            DropPrimaryKey("dbo.BodyTypes");
            AlterColumn("dbo.Images", "YearId", c => c.Int());
            DropColumn("dbo.Years", "Val");
            DropColumn("dbo.Colors", "Val");
            DropColumn("dbo.BodyTypes", "Val");
            AddPrimaryKey("dbo.Years", "Value");
            AddPrimaryKey("dbo.Colors", "Value");
            AddPrimaryKey("dbo.BodyTypes", "Value");
            RenameIndex(table: "dbo.Images", name: "IX_ColorId", newName: "IX_Color_Value");
            RenameIndex(table: "dbo.Images", name: "IX_BodyTypeId", newName: "IX_BodyType_Value");
            RenameIndex(table: "dbo.Images", name: "IX_BrandId", newName: "IX_Brand_Name");
            RenameColumn(table: "dbo.Images", name: "BrandId", newName: "Brand_Name");
            RenameColumn(table: "dbo.Images", name: "YearId", newName: "Year_Value");
            RenameColumn(table: "dbo.Images", name: "ColorId", newName: "Color_Value");
            RenameColumn(table: "dbo.Images", name: "BodyTypeId", newName: "BodyType_Value");
            CreateIndex("dbo.Images", "Year_Value");
            AddForeignKey("dbo.Images", "Year_Value", "dbo.Years", "Value");
            AddForeignKey("dbo.Images", "Color_Value", "dbo.Colors", "Value");
            AddForeignKey("dbo.Images", "BodyType_Value", "dbo.BodyTypes", "Value");
        }
    }
}

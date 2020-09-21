namespace Wallpaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageExtensionToImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "ImageExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "ImageExtension");
        }
    }
}

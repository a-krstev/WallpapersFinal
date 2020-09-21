namespace Wallpaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandedDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                        NumberOfLikes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageEncoding = c.String(),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Brand = c.String(nullable: false),
                        CarName = c.String(nullable: false),
                        BodyType = c.String(),
                        Color = c.String(),
                        Year = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserActions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Action = c.Int(nullable: false),
                        Image_ID = c.Int(),
                        MyUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Images", t => t.Image_ID)
                .ForeignKey("dbo.MyUsers", t => t.MyUser_ID)
                .Index(t => t.Image_ID)
                .Index(t => t.MyUser_ID);
            
            CreateTable(
                "dbo.MyUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        IdentityID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityID)
                .Index(t => t.IdentityID);
            
            CreateTable(
                "dbo.ImageAlbums",
                c => new
                    {
                        Image_ID = c.Int(nullable: false),
                        Album_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_ID, t.Album_ID })
                .ForeignKey("dbo.Images", t => t.Image_ID, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_ID, cascadeDelete: true)
                .Index(t => t.Image_ID)
                .Index(t => t.Album_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyUsers", "IdentityID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserActions", "MyUser_ID", "dbo.MyUsers");
            DropForeignKey("dbo.UserActions", "Image_ID", "dbo.Images");
            DropForeignKey("dbo.ImageAlbums", "Album_ID", "dbo.Albums");
            DropForeignKey("dbo.ImageAlbums", "Image_ID", "dbo.Images");
            DropIndex("dbo.ImageAlbums", new[] { "Album_ID" });
            DropIndex("dbo.ImageAlbums", new[] { "Image_ID" });
            DropIndex("dbo.MyUsers", new[] { "IdentityID" });
            DropIndex("dbo.UserActions", new[] { "MyUser_ID" });
            DropIndex("dbo.UserActions", new[] { "Image_ID" });
            DropTable("dbo.ImageAlbums");
            DropTable("dbo.MyUsers");
            DropTable("dbo.UserActions");
            DropTable("dbo.Images");
            DropTable("dbo.Albums");
        }
    }
}

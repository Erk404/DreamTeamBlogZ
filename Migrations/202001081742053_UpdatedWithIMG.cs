namespace DreamTeamBlogZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedWithIMG : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "ImgUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "ImgUrl");
        }
    }
}

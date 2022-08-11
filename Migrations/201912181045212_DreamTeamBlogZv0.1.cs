namespace DreamTeamBlogZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DreamTeamBlogZv01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MemberSince", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MemberSince");
        }
    }
}

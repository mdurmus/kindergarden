namespace kindergarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveUserSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActiveUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SchoolId = c.Int(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActiveUsers");
        }
    }
}

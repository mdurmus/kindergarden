namespace kindergarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLoggingRecords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        LoginTime = c.DateTime(nullable: false),
                        SchoolName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogRecords");
        }
    }
}

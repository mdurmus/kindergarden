namespace kindergarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRolePropertyLoggingRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogRecords", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogRecords", "Role");
        }
    }
}

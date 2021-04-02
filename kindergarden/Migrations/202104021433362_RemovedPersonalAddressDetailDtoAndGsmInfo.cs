namespace kindergarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPersonalAddressDetailDtoAndGsmInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.person", "Gsm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.person", "Gsm", c => c.String(maxLength: 100));
        }
    }
}

namespace kindergarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        subject = c.String(maxLength: 500),
                        miniText = c.String(maxLength: 1000),
                        fullText = c.String(),
                        adminText = c.String(),
                        startDate = c.DateTime(),
                        finishDate = c.DateTime(),
                        location = c.String(maxLength: 250),
                        isActive = c.Boolean(),
                        SchoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId, cascadeDelete: true)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.activitiesMessage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        activitiesId = c.Int(),
                        messageText = c.String(),
                        isActive = c.Boolean(),
                        messageOwner = c.String(),
                        messageDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.activities", t => t.activitiesId)
                .Index(t => t.activitiesId);
            
            CreateTable(
                "dbo.answerActivityMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        activitiesId = c.Int(nullable: false),
                        messageText = c.String(),
                        isActive = c.Boolean(),
                        messageOwner = c.String(),
                        messageDate = c.DateTime(nullable: false),
                        activitiesMessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.activitiesMessage", t => t.activitiesMessageId, cascadeDelete: true)
                .Index(t => t.activitiesMessageId);
            
            CreateTable(
                "dbo.activitiesPicture",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        activitiesId = c.Int(),
                        filePath = c.String(),
                        isActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.activities", t => t.activitiesId)
                .Index(t => t.activitiesId);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        SchoolId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                        PostalCode = c.String(),
                        City = c.String(),
                        State = c.String(),
                        schoolGuid = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsPayWillSchool = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SchoolId);
            
            CreateTable(
                "dbo.CalendarActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        Text = c.String(),
                        SchoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId, cascadeDelete: true)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.galleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        SchoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId, cascadeDelete: true)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.galleryImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        filePath = c.String(),
                        galleryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.galleries", t => t.galleryId, cascadeDelete: true)
                .Index(t => t.galleryId);
            
            CreateTable(
                "dbo.news",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        subject = c.String(),
                        newsText = c.String(),
                        url = c.String(),
                        filePath = c.String(),
                        priority = c.Int(nullable: false),
                        SchoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId, cascadeDelete: true)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 50),
                        Gsm = c.String(maxLength: 100),
                        Pass = c.String(maxLength: 50),
                        IsAdmin = c.Boolean(),
                        IsTeacher = c.Boolean(),
                        IsMaster = c.Boolean(),
                        IsActive = c.Boolean(),
                        IsParent = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        SchoolId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(),
                        State = c.String(),
                        PostCode = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.person", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.personMessage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsUnRead = c.Boolean(nullable: false),
                        SenderId = c.Int(),
                        PersonId = c.Int(nullable: false),
                        Person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.person", t => t.SenderId)
                .ForeignKey("dbo.person", t => t.Person_Id)
                .Index(t => t.SenderId)
                .Index(t => t.PersonId)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(),
                        Text = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        FileName = c.String(),
                        FullPath = c.String(),
                        sender = c.String(),
                        LeaveDate = c.DateTime(nullable: false),
                        PersonMessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.person", t => t.OwnerId)
                .ForeignKey("dbo.personMessage", t => t.PersonMessageId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.PersonMessageId);
            
            CreateTable(
                "dbo.advertisement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyName = c.String(),
                        filePath = c.String(),
                        url = c.String(maxLength: 250),
                        mapsLink = c.String(maxLength: 250),
                        isActive = c.Boolean(),
                        startDate = c.DateTime(),
                        finishDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.person", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.personMessage", "Person_Id", "dbo.person");
            DropForeignKey("dbo.personMessage", "SenderId", "dbo.person");
            DropForeignKey("dbo.personMessage", "PersonId", "dbo.person");
            DropForeignKey("dbo.Messages", "PersonMessageId", "dbo.personMessage");
            DropForeignKey("dbo.Messages", "OwnerId", "dbo.person");
            DropForeignKey("dbo.Addresses", "PersonId", "dbo.person");
            DropForeignKey("dbo.news", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.galleries", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.galleryImages", "galleryId", "dbo.galleries");
            DropForeignKey("dbo.CalendarActivities", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.activities", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.activitiesPicture", "activitiesId", "dbo.activities");
            DropForeignKey("dbo.answerActivityMessages", "activitiesMessageId", "dbo.activitiesMessage");
            DropForeignKey("dbo.activitiesMessage", "activitiesId", "dbo.activities");
            DropIndex("dbo.Messages", new[] { "PersonMessageId" });
            DropIndex("dbo.Messages", new[] { "OwnerId" });
            DropIndex("dbo.personMessage", new[] { "Person_Id" });
            DropIndex("dbo.personMessage", new[] { "PersonId" });
            DropIndex("dbo.personMessage", new[] { "SenderId" });
            DropIndex("dbo.Addresses", new[] { "PersonId" });
            DropIndex("dbo.person", new[] { "SchoolId" });
            DropIndex("dbo.news", new[] { "SchoolId" });
            DropIndex("dbo.galleryImages", new[] { "galleryId" });
            DropIndex("dbo.galleries", new[] { "SchoolId" });
            DropIndex("dbo.CalendarActivities", new[] { "SchoolId" });
            DropIndex("dbo.activitiesPicture", new[] { "activitiesId" });
            DropIndex("dbo.answerActivityMessages", new[] { "activitiesMessageId" });
            DropIndex("dbo.activitiesMessage", new[] { "activitiesId" });
            DropIndex("dbo.activities", new[] { "SchoolId" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.advertisement");
            DropTable("dbo.Messages");
            DropTable("dbo.personMessage");
            DropTable("dbo.Addresses");
            DropTable("dbo.person");
            DropTable("dbo.news");
            DropTable("dbo.galleryImages");
            DropTable("dbo.galleries");
            DropTable("dbo.CalendarActivities");
            DropTable("dbo.Schools");
            DropTable("dbo.activitiesPicture");
            DropTable("dbo.answerActivityMessages");
            DropTable("dbo.activitiesMessage");
            DropTable("dbo.activities");
        }
    }
}

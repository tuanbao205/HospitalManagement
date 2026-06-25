namespace HospitalManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Doctors", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Rooms", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DoctorSchedules", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorSchedules", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Doctors", new[] { "DepartmentId" });
            DropIndex("dbo.Rooms", new[] { "DepartmentId" });
            DropIndex("dbo.DoctorSchedules", new[] { "DoctorId", "RoomId", "WorkDate", "StartTime" });
            DropColumn("dbo.Doctors", "DepartmentId");
            DropTable("dbo.Departments");
            DropTable("dbo.Rooms");
            DropTable("dbo.DoctorSchedules");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DoctorSchedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        WorkDate = c.DateTime(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Shift = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomNumber = c.String(nullable: false, maxLength: 50),
                        RoomType = c.String(nullable: false, maxLength: 50),
                        DepartmentId = c.Int(),
                        IsAvailable = c.Boolean(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.RoomId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            AddColumn("dbo.Doctors", "DepartmentId", c => c.Int());
            CreateIndex("dbo.DoctorSchedules", new[] { "DoctorId", "RoomId", "WorkDate", "StartTime" }, unique: true);
            CreateIndex("dbo.Rooms", "DepartmentId");
            CreateIndex("dbo.Doctors", "DepartmentId");
            AddForeignKey("dbo.DoctorSchedules", "RoomId", "dbo.Rooms", "RoomId", cascadeDelete: true);
            AddForeignKey("dbo.DoctorSchedules", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
            AddForeignKey("dbo.Rooms", "DepartmentId", "dbo.Departments", "DepartmentId");
            AddForeignKey("dbo.Doctors", "DepartmentId", "dbo.Departments", "DepartmentId");
        }
    }
}

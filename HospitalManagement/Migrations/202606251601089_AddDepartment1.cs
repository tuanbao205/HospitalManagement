namespace HospitalManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepartment1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "Department_DepartmentId", c => c.Int());
            CreateIndex("dbo.Doctors", "Department_DepartmentId");
            AddForeignKey("dbo.Doctors", "Department_DepartmentId", "dbo.Departments", "DepartmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doctors", "Department_DepartmentId", "dbo.Departments");
            DropIndex("dbo.Doctors", new[] { "Department_DepartmentId" });
            DropColumn("dbo.Doctors", "Department_DepartmentId");
        }
    }
}

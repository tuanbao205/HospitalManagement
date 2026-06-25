namespace HospitalManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorDepartmentRelation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Doctors", name: "Department_DepartmentId", newName: "DepartmentId");
            RenameIndex(table: "dbo.Doctors", name: "IX_Department_DepartmentId", newName: "IX_DepartmentId");
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String(nullable: false));
            RenameIndex(table: "dbo.Doctors", name: "IX_DepartmentId", newName: "IX_Department_DepartmentId");
            RenameColumn(table: "dbo.Doctors", name: "DepartmentId", newName: "Department_DepartmentId");
        }
    }
}

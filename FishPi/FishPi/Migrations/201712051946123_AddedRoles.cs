namespace FishPi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRoles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FishPiCalculations", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FishPiCalculations", "Title", c => c.String());
        }
    }
}

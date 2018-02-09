namespace FishPi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPiAsFractionAndFixedTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FishPiCalculations", "PiAsFraction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FishPiCalculations", "PiAsFraction");
        }
    }
}

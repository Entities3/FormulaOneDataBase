namespace Formula1.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UniqueColumnCombinations : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Races", new string[3] { "Season_Id", "Circuit_Id", "Driver_Id" },
       unique: true, name: "UQ_Race_Driver");

            CreateIndex("SeasonParticipants", new string[2] { "Season_Id", "DriverPermanentNumber" },
               unique: true, name: "UQ_Season_DriverPermanentNumber");

            CreateIndex("SeasonParticipants", new string[2] { "Season_Id", "Driver_Id" },
              unique: true, name: "UQ_Season_Driver");
        }

        public override void Down()
        {
            DropIndex("Races", "UQ_Race_Driver");

            DropIndex("SeasonParticipants", "UQ_Season_DriverPermanentNumber");

            DropIndex("SeasonParticipants", "UQ_Season_Driver");
        }
    }
}

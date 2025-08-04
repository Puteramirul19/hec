using System;

namespace Hec.Entities
{
    public class RoomAppliance
    {
        public Guid ApplianceId { get; internal set; }
        public string ApplianceName { get; set; }
        public int NumbersOfApp { get; set; }
        public decimal HoursPerDay { get; set; }
        public decimal DaysPerMonth { get; set; }
        public decimal Watts { get; set; }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;

namespace Hec.Entities
{
    public class House
    {
        public House()
        {
            this.Rooms = new List<Room>();
        }

        public string AccountNo { get; internal set; }
        public string HouseName { get; set; }
        public string HouseType { get; set; }
        public IList<Room> Rooms { get; set; }
        public decimal TotalUsage { get; set; }
        public decimal TotalCost { get; set; }

        public void CalculateUsage()
        {
            //TODO: Check calculation
            TotalUsage = Rooms.SelectMany(x => x.Appliances).Sum(x => x.NumbersOfApp * x.HoursPerDay * x.DaysPerMonth * x.Watts);
            
            //TODO calculation
            TotalCost = 0;
        }
    }
}
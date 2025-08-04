using System.Collections.Generic;

namespace Hec.Entities
{
    public class Room
    {
        public Room()
        {
            this.Appliances = new List<RoomAppliance>();
        }

        public RoomType RoomType { get; set; }
        public string RoomName { get; set; }
        public IList<RoomAppliance> Appliances { get;  set; }
    }
}
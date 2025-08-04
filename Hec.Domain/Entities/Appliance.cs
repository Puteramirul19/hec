using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class Appliance : Entity
    {
        public string Name { get; set; }
        public int DefaultNumbersOfApp { get; set; }
        public decimal DefaultHoursPerDay { get; set; }
        public decimal DefaultDaysPerMonth { get; set; }
        public decimal DefaultWatts { get; set; }
        public bool ForLivingRoom { get; set; }
        public bool ForMasterBedroom { get; set; }
        public bool ForBedroom { get; set; }
        public bool ForBathroom { get; set; }
        public bool ForDiningRoom { get; set; }
        public bool ForKitchen { get; set; }

        public Guid TipCategoryId { get; set; }
        public virtual TipCategory TipCategory { get; set; }

        //add photo
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }

        public bool IsActive { get; set; }

        public static IQueryable<Appliance> GetAppliancesForRoomType(HecContext db, RoomType roomType)
        {
            switch (roomType)
            {
                case RoomType.LivingRoom:
                    return db.Appliances.Where(x => x.ForLivingRoom == true);

                case RoomType.MasterBedroom:
                    return db.Appliances.Where(x => x.ForMasterBedroom == true);

                case RoomType.Bedroom:
                    return db.Appliances.Where(x => x.ForBedroom == true);

                case RoomType.Bathroom:
                    return db.Appliances.Where(x => x.ForBathroom == true);

                case RoomType.DiningRoom:
                    return db.Appliances.Where(x => x.ForDiningRoom == true);

                case RoomType.Kitchen:
                    return db.Appliances.Where(x => x.ForKitchen == true);

                default:
                    return (new Appliance[] {}).AsQueryable();
            }
        }
    }
}

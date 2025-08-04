using Hec.Entities;
using Hec.IdGeneration;
using Hec.Workflows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Hec.Migrations
{
    public partial class Configuration
    {
        private void SeedSampleData(HecContext db)
        {
            var user = db.Users.Find("public1@hec.tnb.com.my");

            // 
            // User 1
            //
            foreach (var ca in db.ContractAccounts.Where(x => x.UserId == user.Id).ToList())
            {
                foreach (var bill in db.Bills.Where(x => x.ContractAccountId == ca.Id).ToList())
                    db.SetDeleted(bill);

                db.SetDeleted(ca);
            }

            db.SaveChanges();

            for (var i=1; i<=3; i++)
            {
                var house = new House()
                {
                    HouseName = "House " + i,
                    HouseType = "Terrace_1"
                };

                var ca = new ContractAccount
                {
                    User = user,
                    AccountNo = "SAMPLECA" + i,
                    PremiseType = "DOM03",
                    House = house,
                    IsFromSsp = (i >= 3) ? false : true,
                    IsDefault = (i == 1),
                };

                for (var j = 1; j <= 3; j++)
                {
                    var room = new Room
                    {
                        RoomName = "Room " + j,
                        RoomType = (RoomType)j
                    };
                    house.Rooms.Add(room);

                    foreach(var app in Appliance.GetAppliancesForRoomType(db, room.RoomType).Take(2))
                    {
                        room.Appliances.Add(new RoomAppliance
                        {
                            ApplianceId = app.Id,
                            ApplianceName = app.Name,
                            NumbersOfApp = app.DefaultNumbersOfApp,
                            HoursPerDay = app.DefaultHoursPerDay,
                            DaysPerMonth = app.DefaultHoursPerDay,
                            Watts = app.DefaultWatts
                        });
                    }
                }

                ca.House.CalculateUsage();
                ca.SerializeData();

                db.ContractAccounts.Add(ca);
            }

            //
            // Friendship
            //

            db.Friendships.Add(new Friendship()
            {
                Inviter = user,
                Invitee = db.Users.Find("public4@hec.tnb.com.my"),
                IsAccepted = true
            });

            db.Friendships.Add(new Friendship()
            {
                Inviter = user,
                Invitee = db.Users.Find("public5@hec.tnb.com.my"),
                IsAccepted = true
            });

            db.Friendships.Add(new Friendship()
            {
                Inviter = user,
                Invitee = db.Users.Find("public6@hec.tnb.com.my"),
                IsAccepted = true
            });

            db.Friendships.Add(new Friendship()
            {
                Inviter = user,
                Invitee = db.Users.Find("public7@hec.tnb.com.my"),
                IsAccepted = true
            });

            db.Friendships.Add(new Friendship()
            {
                Inviter = user,
                Invitee = db.Users.Find("public8@hec.tnb.com.my"),
                IsAccepted = true
            });

            db.SaveChanges();

            //
            // Seed more users
            //

            Action<int> createAccount = (num) =>
            {
                var usr = db.Users.Find("public" + num + "@hec.tnb.com.my");

                foreach (var ca1 in db.ContractAccounts.Where(x => x.UserId == usr.Id).ToList())
                {
                    foreach (var bill in db.Bills.Where(x => x.ContractAccountId == ca1.Id).ToList())
                        db.SetDeleted(bill);

                    db.SetDeleted(ca1);
                }

                var usrCa = new ContractAccount
                {
                    User = usr,
                    AccountNo = "SAMPLECA" + num,
                    PremiseType = "DOM03",
                    House = new House()
                    {
                        HouseName = "House " + num,
                        HouseType = "Terrace_1"
                    },
                    IsFromSsp = true,
                    IsDefault = true
                };
                usrCa.House.CalculateUsage();
                usrCa.SerializeData();
                db.ContractAccounts.Add(usrCa);
                
            };

            createAccount(2);
            createAccount(3);
            createAccount(4);
            createAccount(5);
            createAccount(6);
            createAccount(7);
            createAccount(8);
            createAccount(9);

            db.SaveChanges();
        }
    }
}

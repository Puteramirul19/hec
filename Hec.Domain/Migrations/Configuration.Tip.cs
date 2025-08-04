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

        private void SeedTipsAndCategory(HecContext db)
        {
            db.TipCategories.AddOrUpdate(
                x => x.Id,
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F9785"), Name = "General", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F9786"), Name = "Washing Machine", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F9787"), Name = "Water Heater", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), Name = "Air Conditioner", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F9789"), Name = "Lighting", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F978A"), Name = "Fridge", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F978B"), Name = "Water Pump", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F978C"), Name = "Kettle", IsActive = true },
                new TipCategory { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F978D"), Name = "Microwave", IsActive = false }
            );                             

            db.SaveChanges();

            //db.Tips.AddOrUpdate(
            //    x => x.Id,
            //    //General
            //    new Tip { Id = new Guid("51B6436D-7A3B-4886-8854-944DE078DE6F"), Title = "5-star appliances give you 5-star savings.", Description = "5-star rated appliances are designed to run more efficiently, reducing your consumption.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9785"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Washing Machine
            //    new Tip { Id = new Guid("F6D8A3CA-1CA6-4A52-A540-1FF4C6DF4D47"), Title = "Save smart with a cold wash.", Description = "Hot water makes your washing machine work a little harder. Setting your washing machine to a cold wash whenever possible will help save electricity.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9786"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("C0720EC6-6D50-4E2B-8DDC-7CF1465D7E3D"), Title = "Choose the clothes line over the dryer.", Description = "The heavy electricity consumption of clothes dryers is why the trusted clothes line still serves us till today. Using our sunny weather to dry your clothes will help you save more.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9786"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("C1DE89C2-E81E-4E98-B0A4-FC99923506DC"), Title = "A full load of laundry saves energy. And time", Description = "Washing machines consumes a lot of energy. Full loads will save energy by reducing the amount of times you'll have to do your laundry.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9786"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("37B8D44F-AF8A-4809-99A1-8431C79EAF02"), Title = "Iron in bigger amounts to bring down your energy usage.", Description = "Ironing bigger loads of laundry at once reduces the amount of times you'll have to iron your clothes.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9786"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Water Heater
            //    new Tip { Id = new Guid("84D05B62-ACBC-448F-BAE8-7CED4964963E"), Title = "Switch to instant water heaters to save more energy.", Description = "Unlike tank water heaters, instant water heaters save more by heating up only when in use.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9787"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Air Conditioner
            //    new Tip { Id = new Guid("E337453E-482D-4CA7-A9C0-5431A939AD4C"), Title = "Setting a timer on your air con is a smart way to manage and save energy.", Description = "You do not need the air con running all night to stay cool. Try Setting it to switch off a few hour after you've fallen asleep to be more energy efficient.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("AE99495E-F6B9-4DE6-BA55-72FF94A3C012"), Title = "Dress right for cool savings.", Description = "Dressing appropriately for the weather can help reduce your energy consumption. Choose thinner and breathable clothing to keep cool.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("B45FFAEC-A725-4E07-853F-235E78848304"), Title = "Clean your air con filter to save more. And sweat less.", Description = "Keeping your air conditioner filter clean keeps it running efficiently by allowing more cool air to flow out.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("449078D8-4A35-44A8-BB6F-4A4471510B17"), Title = "Close windows to save energy. And keep cool.", Description = "Close your windows and doors while your air conditioner is on, cools your room faster.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("705440BB-7046-44AC-AE34-FC0A86FF6046"), Title = "Your air con size matters when it comes to energy savings.", Description = "When choosing an air conditioner for your home, remember to buy one that's appropriate for the size of the room.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("24F178A6-A313-42A8-BCB0-B30984390342"), Title = "Shade up your home to bring down your bill.", Description = "Installing shades, drapes and tinting can minimise the entry of external heat into your home, making it easier not to turn on the air conditioner.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("21F4C8CD-B216-4076-9972-8EEE518C9C78"), Title = "Choose fans over air con to save energy.", Description = "Using fans instead of your air conditioners is a simple but very effective way to cut down your energy consumption.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("15B951E6-0DAC-408F-836D-5CCD50F88ADD"), Title = "Set your air con at 24°C for optimum coolness with higher savings.", Description = "Air conditioners consume a lot of energy/ Setting the temperature between 24-26°C provides sufficient cooling, while keeping you energy consumption down.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9788"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Lighting
            //    new Tip { Id = new Guid("0CD758FD-5E2F-438E-820B-252058546B51"), Title = "Brighten up for less with T5 fluorescent lights.", Description = "Slimmer fluorescent lights such as the T5 variants provide ample brightness to light up your home while consuming less energy.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9789"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("85AA2F14-6BD1-47F5-B6B4-29F6A2B3432F"), Title = "Save smart with energy-saving light bulbs.", Description = "Energy-saving light bulbs consume less energy and last longer.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9789"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("E3295C5E-4A84-4195-8153-496D28B8F34D"), Title = "Use spotlights with motion sensors to brighten your savings.", Description = "Installing motion sensors can help save energy by turning on your spotlights only when they need to be.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9789"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("8AFB46AB-903C-4135-A094-E934D3FD230B"), Title = "Open up to natural light to save energy.", Description = "Draw your curtains and blinds to let in as much natural light as possible to help save on internal lighting costs.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9789"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("0F4DE894-52AF-4C19-994F-89ADE37A7A70"), Title = "Use outdoor light timers to prevent excessive energy usage.", Description = "Instead of leaving your outdoor lights on all night, install a timer to keep them on, only when they need to be.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F9789"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Fridge
            //    new Tip { Id = new Guid("2A02AF40-1078-48AC-BA32-EFDE543E3CDB"), Title = "Give your fridge some space to breathe more. And consume less.", Description = "Keep your refrigerator at least 10cm from the wall. and away from warm areas such as the stove. Proper positioning of your refrigerator can help it run more efficiently.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F978A"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("596D7228-524A-4A3C-BFD4-3F25B0C081D0"), Title = "An organised fridge makes room for energy savings.", Description = "An uncluttered fridge allows cool air to circulate more efficiently.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F978A"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Water Pump
            //    new Tip { Id = new Guid("640562F1-5782-41A6-9C14-C0499167E9C9"), Title = "Use energy saving water pumps to pump up your energy savings.", Description = "The water pumps that keep your ponds, aquariums and pools running use up a fair bit of electricity. That's why it is important to get ones with energy efficient motors.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F978B"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("A9DCC21E-FBF5-4218-8C1F-62AC233661CE"), Title = "Manage your water pump to manage your savings.", Description = "Pool or pond pumps consume a lot of energy as they need to be on for longer periods of time. Turning them off when not in use will cut down your electricity usage.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F978B"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    //Kettle
            //    new Tip { Id = new Guid("0EFF9F37-2327-41A3-B767-A29DDC51ED5B"), Title = "Fill your kettle to the max level to maximise your energy savings.", Description = "Make the most out of every boil with a full kettle. And to save even more, store your boiled water in a flask to keep it warm for longer.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F978C"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true },
            //    new Tip { Id = new Guid("F0C71852-0268-4EF7-8241-54595D8F9736"), Title = "Keep your kettle clean to enjoy more savings.", Description = "Lime-scale on your kettle's heater element slows down the heating process. Clean it regularly to keep it running more efficiently.", TipCategoryId = new Guid("F0C71852-0268-4EF7-8241-54595D8F978C"), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, IsActive = true }

            //);

            //db.SaveChanges();

            //var c = 1;
            //var dt = DateTime.Now.ToString("yyyy-MM-dd");
            //foreach (var cat in db.TipCategories)
            //{
            //    db.Tips.Add(new Tip
            //    {
            //        Title = "Sample tips " + c.ToString("000"),
            //        Description = $"Tips for <b>{cat.Name}</b> {dt}",
            //        TipCategoryId = cat.Id
            //    });

            //    c++;

            //    db.Tips.Add(new Tip
            //    {
            //        Title = "Sample tips " + c.ToString("000"),
            //        Description = $"Tips for <b>{cat.Name}</b> {dt}",
            //        TipCategoryId = cat.Id
            //    });

            //    c++;
            //}

            //db.SaveChanges();
        }
    }
}

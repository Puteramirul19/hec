using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hec.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using SpreadsheetLight;
using Hec.FileStorage;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class OffDayController : BaseController
    {
        private IFileStore fileStore;

        public OffDayController(HecContext db, IFileStore fileStore)
            : base(db)
        {
            this.fileStore = fileStore;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileUpload(int id /*year*/)
        {
            ViewBag.Year = id;
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(db.OffDays.ToDataSourceResult(request));
        }

        private async Task<OffDay> TryFind(Guid id)
        {
            var entity = await db.OffDays.FindAsync(id);
            if (entity == null)
                throw new IdNotFoundException<OffDay>(id);
            return entity;
        }

        //public async Task<ActionResult> Read (Guid id)
        //{
        //    return Json(await TryFind(id));
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Update(Guid id, OffDay model)
        {
            //CheckAccess(AccessRights.ManageSettings);

            var entity = await TryFind(model.Id);

            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            if (id != model.Id)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            entity.Date = model.Date;
            entity.Year = model.Date.Year;
            entity.Name = model.Name;
            entity.StateId = model.StateId;

            db.SetModified(entity);
            await db.SaveChangesAsync();

            return Json(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Create(OffDay model)
        {
            //CheckAccess(AccessRights.ManageSettings);

            if (!ModelState.IsValid)
            {
                return Status(HttpStatusCode.BadRequest);
            }

            var entity = new OffDay();
            entity.Date = model.Date;
            entity.Year = model.Date.Year;
            entity.Name = model.Name;
            entity.StateId = model.StateId;

            db.OffDays.Add(model);
            await db.SaveChangesAsync();

            model.Id = entity.Id;

            return Json(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var entity = await TryFind(id);

            try
            {
                db.SetDeleted(entity);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Cannot delete item. It might already be used in the system.", ex);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Download(int id /*year*/)
        {
            var states = db.States.OrderBy(x => x.Name).ToArray();
            var offdays = db.OffDays.OrderBy(x => x.Date).Where(x => x.Year == id).ToList();

            var days = new List<Day>();
            foreach (var offday in offdays)
            {
                var existing = days.FirstOrDefault(x => x.Name == offday.Name && x.Date == offday.Date);
                if (existing != null)
                {
                    if (!existing.States.Contains(offday.StateId))
                        existing.States.Add(offday.StateId);
                }
                else
                {
                    var newday = new Day
                    {
                        Name = offday.Name,
                        Date = offday.Date
                    };
                    newday.States.Add(offday.StateId);

                    days.Add(newday);
                }
            }

            var doc = new SLDocument();

            var boldStyle = doc.CreateStyle();
            boldStyle.Font.Bold = true;
            doc.SetRowStyle(1, boldStyle);

            doc.SetCellValue(1, 1, "Holiday Name");
            doc.SetCellValue(1, 2, "Date");

            for (var i = 0; i < states.Count(); i++)
            {
                doc.SetCellValue(1, 3 + i, states[i].Name);
            }

            var dateStyle = doc.CreateStyle();
            dateStyle.FormatCode = "dd/mm/yyyy";
            for (var j = 0; j < days.Count(); j++)
            {
                var rownum = 2 + j;
                var day = days[j];

                doc.SetCellValue(rownum, 1, day.Name);

                doc.SetCellValue(rownum, 2, day.Date.ToString("dd/MM/yyyy"));
                doc.SetCellStyle(rownum, 2, dateStyle);

                for (var i = 0; i < states.Count(); i++)
                {
                    if (day.States.Contains(states[i].Id))
                        doc.SetCellValue(rownum, 3 + i, "Y");
                }
            }

            // Write to output stream
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=OffDay_" + id + ".xlsx");
            doc.SaveAs(Response.OutputStream);
            Response.End();

            return null;
        }

        public ActionResult DownloadEhrms(int id /*year*/)
        {
            var states = db.States.OrderBy(x => x.Name).ToArray();
            var offdays = db.OffDays.Include(x => x.State)
                .OrderBy(x => x.Date)
                .OrderBy(x => x.State.EhrmsCode)
                .Where(x => x.Year == id).ToList();

            var doc = new SLDocument();

            // Headers
            var boldStyle = doc.CreateStyle();
            boldStyle.Font.Bold = true;
            doc.SetRowStyle(1, boldStyle);

            doc.SetCellValue(1, 1, "HOL_CALENDAR");
            doc.SetCellValue(1, 2, "DATE");
            doc.SetCellValue(1, 3, "TXT_SHORT");
            doc.SetCellValue(1, 4, "TXT_LONG");

            // Contents
            var dateStyle = doc.CreateStyle();
            dateStyle.FormatCode = "dd/mm/yyyy";

            for (var j = 0; j < offdays.Count(); j++)
            {
                var rownum = 2 + j;
                var day = offdays[j];

                doc.SetCellValue(rownum, 1, day.State.EhrmsCode);

                doc.SetCellValue(rownum, 2, day.Date.ToString("dd.MM.yyyy"));
                doc.SetCellStyle(rownum, 2, dateStyle);

                doc.SetCellValue(rownum, 3, "");
                doc.SetCellValue(rownum, 4, day.Name);
            }

            // Write to output stream
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=OffDay_" + id + ".xlsx");
            doc.SaveAs(Response.OutputStream);
            Response.End();

            return null;
        }

        private class Day
        {
            public string Name;
            public DateTime Date;
            public List<Guid> States = new List<Guid>();
        }

        public ActionResult Upload(int year, string fileId)
        {
            var records = ProcessEhrmsFile(year, fileId);

            return Json(new
            {
                successCount = records.Count(x => x.Ok == true),
                failCount = records.Count(x => x.Ok != true),
                records = records
            });
        }

        private IEnumerable<Record> ProcessFile(int year, string fileId)
        {
            var records = new List<Record>();

            using (var stream = fileStore.GetStream(fileId))
            {
                var doc = new SLDocument(stream);

                var dbStates = db.States.OrderBy(x => x.Name).ToArray();
                var states = new Dictionary<Guid, string>();

                for (var i = 0; i < int.MaxValue; i++)
                {
                    var val = doc.GetCellValueAsString(1, 3 + i);
                    if (String.IsNullOrEmpty(val))
                        break;

                    var dbState = dbStates.FirstOrDefault(x => x.Name == val);
                    if (dbState != null)
                        states.Add(dbState.Id, dbState.Name);
                    else
                        states.Add(Guid.NewGuid(), null); // just add Guid.Empty into the list to indicate unknown state
                }

                var INVALID_DATE = new DateTime(1900, 1, 1); // Date if parse date failed by SpreadsheetLight

                for (var j = 0; j < int.MaxValue; j++)
                {
                    var rownum = 2 + j;

                    var name = doc.GetCellValueAsString(rownum, 1);
                    if (String.IsNullOrEmpty(name))
                        break;

                    var date = DateTime.MinValue;
                    var dateStr = doc.GetCellValueAsString(rownum, 2);
                    if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                    {
                        records.Add(new Record
                        {
                            RowNum = rownum,
                            Ok = false,
                            Name = name,
                            Remarks = "Invalid date."
                        });
                        continue;
                    }

                    //var date = doc.GetCellValueAsDateTime(rownum, 2);
                    //if (date == INVALID_DATE)
                    //{
                    //    records.Add(new Record
                    //    {
                    //        RowNum = rownum,
                    //        Ok = false,
                    //        Name = name,
                    //        Remarks = "Invalid date."
                    //    });
                    //    continue;
                    //}

                    if (date.Year != year)
                    {
                        records.Add(new Record
                        {
                            RowNum = rownum,
                            Ok = false,
                            Date = date,
                            Name = name,
                            Remarks = "Date is not in year " + year + "."
                        });
                        continue;
                    }

                    var i = 0;
                    foreach (var state in states)
                    {
                        var val = doc.GetCellValueAsString(rownum, 3 + i);
                        if (val == "Y")
                        {
                            var rec = new Record
                            {
                                RowNum = rownum,
                                Name = name,
                                Date = date
                            };
                            if (state.Value == null)
                            {
                                rec.Ok = false;
                                rec.Remarks = "Unknown state";
                            }
                            else
                            {
                                rec.Ok = true;
                                rec.StateId = state.Key;
                                rec.StateName = state.Value;
                            }
                            records.Add(rec);
                        }
                        i++;
                    }
                }

                if (records.All(x => x.Ok == true))
                {
                    // Delete old values
                    foreach (var oldday in db.OffDays.Where(x => x.Year == year))
                        db.Entry(oldday).State = System.Data.Entity.EntityState.Deleted;

                    // Add newly uploaded ones
                    foreach (var rec in records.Where(x => x.Ok == true))
                    {
                        db.OffDays.Add(new OffDay
                        {
                            Name = rec.Name,
                            Date = rec.Date,
                            StateId = rec.StateId,
                            Year = year
                        });
                    }

                    db.SaveChanges();
                }
            }

            return records;
        }

        private IEnumerable<Record> ProcessEhrmsFile(int year, string fileId)
        {
            var records = new List<Record>();

            using (var stream = fileStore.GetStream(fileId))
            {
                var doc = new SLDocument(stream);

                var dbStates = db.States.OrderBy(x => x.EhrmsCode).ToArray();
                
                var INVALID_DATE = new DateTime(1900, 1, 1); // Date if parse date failed by SpreadsheetLight

                for (var rownum = 2; rownum < int.MaxValue; rownum++) // Skip header at row 1
                {
                    var stateCode = doc.GetCellValueAsString(rownum, 1);
                    if (String.IsNullOrEmpty(stateCode))
                        break;

                    var dateStr = doc.GetCellValueAsString(rownum, 2);
                    var holidayName = doc.GetCellValueAsString(rownum, 4);

                    stateCode = stateCode.Trim();

                    // State might share EHRMS code. E.g  KL vs Putrajaya/Cyberjaya
                    var foundStates = dbStates.Where(x => x.EhrmsCode == stateCode);

                    if (foundStates == null || foundStates.Count() == 0)
                    {
                        records.Add(new Record
                        {
                            RowNum = rownum,
                            Ok = false,
                            Name = holidayName,
                            Remarks = "State code '" + stateCode + "' not recognized."
                        });
                        continue;
                    }

                    var date = DateTime.MinValue;
                    if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                    {
                        records.Add(new Record
                        {
                            RowNum = rownum,
                            Ok = false,
                            Name = holidayName,
                            Remarks = "Invalid date."
                        });
                        continue;
                    }

                    if (date.Year != year)
                    {
                        records.Add(new Record
                        {
                            RowNum = rownum,
                            Ok = false,
                            Date = date,
                            Name = holidayName,
                            Remarks = "Date is not in year " + year + "."
                        });
                        continue;
                    }

                    // All OK
                    foreach (var state in foundStates)
                        records.Add(new Record
                        {
                            Ok = true,
                            RowNum = rownum,
                            Name = holidayName,
                            Date = date,
                            StateId = state.Id,
                            StateName = state.Name
                        });

                }

                if (records.Where(x => x.Ok == true).Count() > 0)
                {
                    // Delete old values
                    foreach (var oldday in db.OffDays.Where(x => x.Year == year))
                        db.Entry(oldday).State = System.Data.Entity.EntityState.Deleted;

                    // Add newly uploaded ones
                    foreach (var rec in records.Where(x => x.Ok == true))
                    {
                        db.OffDays.Add(new OffDay
                        {
                            Name = rec.Name,
                            Date = rec.Date,
                            StateId = rec.StateId,
                            Year = year
                        });
                    }

                    db.SaveChanges();
                }
            }

            return records;
        }

        private class Record
        {
            public int RowNum;
            public bool Ok;
            public string Name;
            public DateTime Date;
            public Guid StateId;
            public string StateName;
            public string Remarks;
        }
    }
}

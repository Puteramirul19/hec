//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Tables;
//using MigraDoc.Rendering;
//using Hec.Entities;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using MigraDoc.DocumentObjectModel.Shapes;
//using System.Data.Entity;

//namespace Hec.Web.Controllers
//{
//    [Authorize]
//    public class PdfController : BaseController
//    {
//        public PdfController(HecContext db): base(db)
//        {
//            this.db = db;
//        }

//        public async Task<Notice> TryFindNotice(Guid id)
//        {
//            var entity = await db.Notices
//                                .Include(x => x.Author)
//                                .Include(x => x.Reviewer)
//                                .Include(x => x.ShutdownReason)
//                                .Include(x => x.Interruption)
//                                .Include(x => x.SubZone)
//                                .Include(x => x.AffectedAreas)
//                                .Include(x => x.AffectedAreas.Select(y => y.SubStation))
//                                .FirstOrDefaultAsync(x => x.Id ==id);
//            if (entity == null)
//                throw new IdNotFoundException<Notice>(id);
//            return entity;
//        }

        
//        private Section CreateDocumentSection()
//        {
//            var document = new Document();

//            // Get the predefined style Normal.
//            Style style = document.Styles["Normal"];
//            style.ParagraphFormat.SpaceBefore = "7mm";
//            style.ParagraphFormat.SpaceAfter = "3mm";
//            style.Font.Name = "Times New Roman";
//            style.Font.Size = "12pt";

//            //style = document.Styles[StyleNames.Header];
//            //style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

//            //style = document.Styles[StyleNames.Footer];
//            //style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

//            // Create a new style called Table based on style Normal
//            style = document.Styles.AddStyle("Table", "Normal");
//            style.ParagraphFormat.SpaceBefore = "1mm";
//            style.ParagraphFormat.SpaceAfter = "1mm";

//            Section section = document.AddSection();

//            section.Document.DefaultPageSetup.Orientation = Orientation.Portrait;

//            // Get the A4 page size
//            MigraDoc.DocumentObjectModel.Unit width, height;
//            PageSetup.GetPageSize(PageFormat.A4, out width, out height);

//            // Each MigraDoc document needs at least one section.

//            var margin = "2cm";
//            section.PageSetup.PageHeight = height;
//            section.PageSetup.PageWidth = width;
//            section.PageSetup.LeftMargin = margin;
//            section.PageSetup.RightMargin = margin;
//            section.PageSetup.TopMargin = margin;
//            section.PageSetup.BottomMargin = margin;

//            return section;
//        }

//        private void RenderDocument(Document document)
//        {
//            //-------------------------------------------------------------------------------- 
//            // Send PDF to browser

//            using (var stream = new MemoryStream())
//            {
//                // Create a renderer
//                var pdfRenderer = new PdfDocumentRenderer();
//                pdfRenderer.Document = document;
//                pdfRenderer.RenderDocument();
//                pdfRenderer.PdfDocument.Save(stream, false);

//                Response.Clear();
//                Response.ContentType = "application/pdf";
//                //Response.AddHeader("Content-Disposition", "attachment; filename" + fileName + ".pdf");
//                Response.AddHeader("content-length", stream.Length.ToString());
//                Response.BinaryWrite(stream.ToArray());
//                Response.Flush();
//                stream.Close();
//                Response.End();
//            }
//        }

//        private Table AddTable(Section section, int firstColWidth)
//        {
//            var tbl = section.AddTable();
//            tbl.Style = "Table";
//            tbl.Borders.Width = 0;
//            tbl.AddColumn(firstColWidth.ToString() + "cm");
//            tbl.AddColumn((16 - firstColWidth).ToString() + "cm");
//            return tbl;
//        }

//        private Row AddRow(Table tbl, string col1, string col2)
//        {
//            var row = tbl.AddRow();
//            row.Cells[0].AddParagraph(col1);
//            row.Cells[0].Format.Font.Bold = true;
//            row.Cells[1].AddParagraph(col2 ?? "");
//            return row;
//        }

//        private Paragraph AddPara(Section section, string text, bool isBold, bool isCenter)
//        {
//            var para = section.AddParagraph(text ?? "");
//            para.Format.Font.Bold = isBold;
//            if (isCenter)
//                para.Format.Alignment = ParagraphAlignment.Center;
//            return para;
//        }

//        public async Task ShutdownNotice(Guid id)
//        {
//            // Get Notice
//            var notice = await TryFindNotice(id);

//            Section section = CreateDocumentSection();

//            var refTable = AddTable(section, 4);
//            AddRow(refTable, "Rujukan Kami :", $"TNBD/SNM/{notice.RefNo}");


//            //AddRow(refTable, "Tarikh :", notice.ReviewedOn.HasValue ? notice.ReviewedOn.Value.ToString("dd/MM/yyyy") : "");

//            AddRow(refTable, "Tarikh :", DateTime.Now.ToString("dd/MM/yyyy"));

//            AddPara(section, "Kepada Pelanggan Yang Kami Hargai", true, false);

//            var titlePara = section.AddParagraph(notice.Title.ToUpper());
//            titlePara.Format.Font.Size = "14pt";
//            titlePara.Format.Font.Bold = true;
//            titlePara.Format.Font.Underline = Underline.Single;
//            titlePara.Format.Alignment = ParagraphAlignment.Center;

//            AddPara(section, "Kami memohon perhatian anda berhubung dengan gangguan bekalan elektrik yang akan berlaku di kawasan anda seperti berikut :", false, false);

//            var detailTable = AddTable(section, 4);
//            AddRow(detailTable, "Tarikh Gangguan :", notice.StartOn.ToString("dd/MM/yyyy") +" - " + notice.EndOn.ToString("dd/MM/yyyy"));
//            AddRow(detailTable, "Sebab Gangguan :", notice.ShutdownReason.Text +" - "+ (notice.ShutdownReason.NeedRemarks ? notice.ShutdownReasonRemarks : notice.ShutdownReason.Text));
//            AddRow(detailTable, "Masa :", notice.StartOn.ToString("h:mm tt") + " hingga " + 
//                (notice.StartOn.Date == notice.EndOn.Date ? notice.EndOn.ToString("h:mm tt") : notice.EndOn.ToString("h:mm tt (dd/MM/yyyy)")));
        
//            AddRow(detailTable, "Kawasan Terlibat :", String.Join(Environment.NewLine, notice.AffectedAreas.Select(x => String.IsNullOrEmpty(x.Area) ? x.SubStation.Name : x.Area)));

//            var interuption = "";
//            if (notice.Interruption.WithGenSet.Equals(true))
//                interuption += "GANGGUAN BEKALAN SEMENTARA DENGAN GENSET";
//            else if (notice.Interruption.WithGenSet.Equals(false) && notice.Interruption.Sequence.Equals(2))
//                interuption += "GANGGUAN BEKALAN KESELURUHAN";
//            else if (notice.Interruption.WithGenSet.Equals(false) && notice.Interruption.Sequence.Equals(1))
//                interuption += "TIADA GANGGUAN BEKALAN";

//            //Reformat Form B & NotisGangguan - 9/3/2017
//            //AddRow(detailTable, "Jenis Gangguan :", interuption);

//            AddPara(section, "Anda bolehlah berhubung dengan pihak pengurusan tempatan TNB di talian seperti yang tertera di bawah sekiranya maklumat lanjut diperlukan.", false, false);

//            AddPara(section, "Segala kesulitan yang timbul di pihak anda adalah amat dikesali.", false, false);

//            var sigTable = section.AddTable();
//            sigTable.Style = "Table";
//            sigTable.AddColumn("16cm");

//            var manager = db.UserRoles
//                                .Include(x => x.Role)
//                                .Include(x => x.Zone)
//                                .Where(x => x.ZoneId.HasValue && x.ZoneId.Value == notice.SubZone.ZoneId)
//                                .ToList()
//                                .Where(x => x.Role.HaveAccessRight(AccessRights.NoticeSlaBreachedNotification))
//                                .Select(x => x.User)
//                                .FirstOrDefault();

//            var sigRow = sigTable.AddRow();
//            sigRow.Cells[0].AddParagraph("...................................");
//            sigRow.Height = "2cm";
//            sigRow.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;

//            /*Addded - SNM Error after LIVE #2 - Replace Manager to Reviewer */
//            sigTable.AddRow().Cells[0].AddParagraph("(" + notice.Reviewer.FullName + ")");

//            if (!String.IsNullOrEmpty(notice.Reviewer.Designation))
//                sigTable.AddRow().Cells[0].AddParagraph(notice.Reviewer.Designation);

//            /*Remark - SNM Error after LIVE #2*/
//            /*if (manager != null)
//            {
//                sigTable.AddRow().Cells[0].AddParagraph("(" + manager.FullName + ")");

//                if (!String.IsNullOrEmpty(manager.Designation))
//                    sigTable.AddRow().Cells[0].AddParagraph(manager.Designation);
//            }*/

//            sigTable.AddRow().Cells[0].AddParagraph("Bahagian Pembahagian TNB");
//            sigTable.AddRow().Cells[0].AddParagraph("");
//            sigTable.AddRow().Cells[0].AddParagraph("Pegawai Untuk Dihubungi");

//            //Reformat Form B & NotisGangguan - 9/3/2017
//            //sigTable.AddRow().Cells[0].AddParagraph("Author / hp no: " + notice.Author.FullName + " / " + notice.Author.PhoneNumber);
//            //sigTable.AddRow().Cells[0].AddParagraph("Reviewer / hp no:" + notice.Reviewer.FullName + " / " + notice.Reviewer.PhoneNumber);

//            sigTable.AddRow().Cells[0].AddParagraph("Nama: TNB Careline");

//            var phone = "Telefon: 15454";

//            sigTable.AddRow().Cells[0].AddParagraph(phone);

//            RenderDocument(section.Document);
//        }

//        public async Task FormB_BM(Guid id)
//        {
//            // Get Notice
//            var notice = await TryFindNotice(id);

//            Section section = CreateDocumentSection();

//            AddPara(section, "Borang B", true, true);
//            AddPara(section, "[subperaturan 6A(2)]", false, true);
//            AddPara(section, "MALAYSIA", false, true);

//            if (notice.AffectedAreas.Count() > 0)
//                AddPara(section, "NEGERI " + notice.AffectedAreas.First().SubStation.Station.State.Name.ToUpper(), false, true);

//            AddPara(section, "AKTA BEKALAN ELEKTRIK 1990", false, true);
//            AddPara(section, "", false, false);
//            AddPara(section, "Kepada Pengguna Yang DiHormati,", false, false);
//            AddPara(section, "NOTIS PERBENTIAN SEMENTARA / GANGGUAN SEMENTARA BEKALAN ELEKTRIK", true, false);
//            AddPara(section, $"Dimaklumkan bahawa bekalan elektrik di kawasan tuan akan diberhentikan / diganggu sementara pada {notice.StartOn.ToString("dd/MM/yyyy")} hingga {notice.EndOn.ToString("dd/MM/yyyy")} dari {notice.StartOn.ToString("h:mm tt")} hingga {(notice.StartOn.Date == notice.EndOn.Date ? notice.EndOn.ToString("h:mm tt") : notice.EndOn.ToString("h:mm tt"))} bagi tujuan \"{notice.ShutdownReason.Text} - {notice.ShutdownReasonRemarks}\". Sila maklum bahawa \"{notice.Interruption.Description}\" ketika hentitugas sementara ini dijalankan.", false, false);
//            AddPara(section, "", false, false);                                                                    

//            var detailTable = AddTable(section, 6);
//            AddRow(detailTable, "Kawasan yang terlibat :", String.Join(Environment.NewLine, notice.AffectedAreas.Select(x => String.IsNullOrEmpty(x.Area) ? x.SubStation.Name : x.Area)));
//            AddRow(detailTable, "Nama Pemegang Lesen :", "Tenaga Nasional Berhad(Company No.200866 - W)");
//            AddRow(detailTable, "Alamat Pihak Pengurusan Tempatan :", $"Pejabat Ketua Zon {notice.SubZone.Zone.Name}, {notice.SubZone.Name}\nBahagian Pembahagian TNB");
//            AddRow(detailTable, "Pegawai Untuk Dihubungi :", notice.Reviewer.FullName);
//            AddRow(detailTable, "No Telefon :", (String.IsNullOrEmpty(notice.Author.PhoneNumber) ? "" : notice.Author.PhoneNumber + " & ")
//                                                + (String.IsNullOrEmpty(notice.Reviewer.PhoneNumber) ? "" : notice.Reviewer.PhoneNumber + " / ")
//                                                + "15454");
//            AddRow(detailTable, "Tarikh :", DateTime.Now.ToString("dd/MM/yyyy"));

//            RenderDocument(section.Document);
//        }

//        public async Task FormB_BI(Guid id)
//        {
//            // Get Notice
//            var notice = await TryFindNotice(id);

//            Section section = CreateDocumentSection();

//            AddPara(section, "Form B", true, true);
//            AddPara(section, "[subregulation 6A(2)]", false, true);
//            AddPara(section, "MALAYSIA", false, true);

//            if (notice.AffectedAreas.Count() > 0)
//                AddPara(section, "STATE OF " + notice.AffectedAreas.First().SubStation.Station.State.Name.ToUpper(), false, true);

//            AddPara(section, "ELECTRICITY SUPPLY ACT 1990", false, true);
//            AddPara(section, "", false, false);
//            AddPara(section, "Dear Valued Customers,", false, false);
//            AddPara(section, "NOTICE OF TEMPORARY CESSATION / INTERRUPTION OF ELECTRICITY SUPPLY", true, false);
//            AddPara(section, $"Please be informed that the supply of electricity at your respective area shall be temporarily ceased / interrupted on {notice.StartOn.ToString("dd/MM/yyyy")} to {notice.EndOn.ToString("dd/MM/yyyy")} from {notice.StartOn.ToString("h:mm tt")} to {(notice.StartOn.Date == notice.EndOn.Date ? notice.EndOn.ToString("h:mm tt") : notice.EndOn.ToString("h:mm tt"))} for the purpose of \"{notice.ShutdownReason.Text} - {notice.ShutdownReasonRemarks}\". Kindly note that, there will be \"{notice.Interruption.Description}\" to ensure reliability of electricity supply.", false, false);
//            //{notice.StartOn.ToString("h:mm tt")}

//            AddPara(section, "", false, false);
            
//            var detailTable = AddTable(section, 5);
//            AddRow(detailTable, "Affected Areas :", String.Join(Environment.NewLine, notice.AffectedAreas.Select(x => String.IsNullOrEmpty(x.Area) ? x.SubStation.Name : x.Area)));
//            AddRow(detailTable, "Name of License :", "Tenaga Nasional Berhad(Company No.200866 - W)");
//            AddRow(detailTable, "Address :", $"Head of Zone Office {notice.SubZone.Zone.Name}, {notice.SubZone.Name}\nBahagian Pembahagian TNB");
//            AddRow(detailTable, "Officer In-Charge :", "TNB Careline");
//            AddRow(detailTable, "Telephone Number :", "15454");
//            //Reformat Form B & NotisGangguan - 9/3/2017
//            //AddRow(detailTable, "Officer In-Charge :", notice.Reviewer.FullName);
//            //AddRow(detailTable, "Telephone Number :", (String.IsNullOrEmpty(notice.Author.PhoneNumber) ? "" : notice.Author.PhoneNumber + " & ")
//            //                                    + (String.IsNullOrEmpty(notice.Reviewer.PhoneNumber) ? "" : notice.Reviewer.PhoneNumber + " / ")
//            //                                    + "15454");
//            AddRow(detailTable, "Dated :", DateTime.Now.ToString("dd/MM/yyyy"));
            
//            RenderDocument(section.Document);
//        }
//    }

//}
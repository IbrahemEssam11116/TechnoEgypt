using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QRCoder;
using System.Drawing;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;

namespace TechnoEgypt.Services
{
    public class Certificate
    {
        private readonly UserDbContext _dBContext;
        private readonly IWebHostEnvironment env;

        public Certificate(UserDbContext dBContext, IWebHostEnvironment env)
        {
            _dBContext = dBContext;
            this.env = env;
        }
        public (byte[], string) CreateCertificate(int userc)
        {

            var usercoursedata = _dBContext.childCourses.Where(w => w.Id == userc).Include(w => w.Course).Include(w => w.Child).ThenInclude(w => w.parent).FirstOrDefault();
            if (usercoursedata == null)
            {
                return (null, null);
            }
            string UserName = usercoursedata.Child.Name + " " + usercoursedata.Child.parent.FatherTitle;
            string CourseName = usercoursedata.Course.Name;
            var Cdate = usercoursedata.CertificationDate;
            string oldFile = env.WebRootPath + "\\Files\\certificate-2023-1.pdf";
            string watermarkedFile = env.WebRootPath + "\\Files\\new.pdf";
            string pathfontuser = env.WebRootPath + "\\Font\\Poppins-ExtraBold.ttf";
            string pathfontcourse = env.WebRootPath + "\\Font\\Poppins-ExtraBold.ttf";

            // Creating watermark on a separate layer
            // Creating iTextSharp.text.pdf.PdfReader object to read the Existing PDF Document
            PdfReader reader1 = new PdfReader(oldFile);
            iTextSharp.text.Image qr = GenerateQrCodeData(usercoursedata);
            qr.ScaleAbsolute(15, 15);
            using (MemoryStream ms = new MemoryStream())
            {

                // Creating iTextSharp.text.pdf.PdfStamper object to write Data from iTextSharp.text.pdf.PdfReader object to FileStream object
                using (PdfStamper stamper = new PdfStamper(reader1, ms, '\0', true))
                {
                    // Getting total number of pages of the Existing Document
                    //int pageCount = reader1.NumberOfPages;

                    // Create New Layer for Watermark
                    PdfLayer layer = new PdfLayer("Layer", stamper.Writer);
                    // Loop through each Page

                    // Getting the Page Size
                    iTextSharp.text.Rectangle rect = reader1.GetPageSize(1);

                    // Get the ContentByte object
                    PdfContentByte cb = stamper.GetOverContent(1);

                    // Tell the cb that the next commands should be "bound" to this new layer
                    cb.BeginLayer(layer);

                    BaseFont bfuser = BaseFont.CreateFont(@pathfontuser, "Identity-H", BaseFont.EMBEDDED);
                    cb.SetFontAndSize(bfuser, 20);
                    cb.SetRGBColorFill(6, 145, 207);
                    cb.BeginText();

                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, UserName, rect.Width - 620, rect.Height - 360, 0);
                    BaseFont bfcourse = BaseFont.CreateFont(@pathfontcourse, "Identity-H", BaseFont.EMBEDDED);
                    cb.SetFontAndSize(bfcourse, 20);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CourseName, rect.Width - 620, rect.Height - 465, 0);
                    cb.SetFontAndSize(bfuser, 20);
                    cb.SetRGBColorFill(35, 31, 32);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Cdate.ToShortDateString(), rect.Width - 740, rect.Height - 570, 0);
                    cb.AddImage(qr, 100, 0, 0, 100, 500, 15);
                    cb.EndText();

                    // Close the layer
                    cb.EndLayer();

                }
                return (ms.ToArray(), $"{UserName}_{CourseName}.pdf");
            }

        }
        private iTextSharp.text.Image GenerateQrCodeData(ChildCourse usercoursedata)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();

            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(JsonConvert.SerializeObject(new { id = usercoursedata.Course.Id, name = usercoursedata.Course.Name }), QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCoder.QRCode(QrCodeInfo);

            Bitmap QrBitmap = QrCode.GetGraphic(60);
            return iTextSharp.text.Image.GetInstance(QrBitmap, BaseColor.WHITE);

            //byte[] BitmapArray = BitmapToByteArray(QrBitmap);
            //return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));

        }
        //private static byte[] BitmapToByteArray(Bitmap bitmap)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bitmap.Save(ms, ImageFormat.Png);
        //        return ms.ToArray();
        //    }
        //}

        public (byte[], string) CreateCV(int userId)
        {
            userId = 1;
            var userdata = _dBContext.children.Where(w => w.Id == userId).Include(w => w.parent).Include(w => w.ChildCourses).ThenInclude(w => w.Course).FirstOrDefault();
            if (userdata == null)
            {
                return (null, null);
            }


            string oldFile = env.WebRootPath + "\\Files\\Studentcv.pdf";
            string watermarkedFile = env.WebRootPath + "\\Files\\new.pdf";


            // Creating watermark on a separate layer
            // Creating iTextSharp.text.pdf.PdfReader object to read the Existing PDF Document

            // Dynamic info

            string UserName = userdata.Name + " " + userdata.parent.FatherTitle;
            var IQTestList = _dBContext.childCVData.Where(w => (w.ChildId == userId) && (w.stationId == StationType.IqTest)).OrderBy(w => w.Date).LastOrDefault();
            var VolunteerList = _dBContext.childCVData.Where(w => (w.ChildId == userId) && (w.stationId == StationType.Volunteer)).ToList();
            var HighSchoolList = _dBContext.childCVData.Where(w => (w.ChildId == userId) && (w.stationId == StationType.HighSchool)).OrderBy(w => w.Date).LastOrDefault();
            var LanguageList = _dBContext.childCVData.Where(w => (w.ChildId == userId) && (w.stationId == StationType.LanguageTests)).ToList();
            var InternshipsList = userdata.ChildCourses.ToList();
            string SchoolName = userdata.SchoolName;
            string schoolsystem = "International";
            string schoolreport = "Excellent";
            string reportdate = "01/01/2022";
            string reportnotes = "noteshere";
            //string CourseName = usercoursedata.Course.Name;
            //var Cdate = usercoursedata.CertificationDate;








            // Static Infromations

            //string firstName = "Emam";
            //string FatherTitle = "Mohammed Ibrahim Mohammed";
            //string SchoolName = "Borg El Arab Primary School";
            //string schoolsystem = "International";
            //string schoolreport = "Excellent";
            //string reportdate = "01/01/2022";
            //string reportnotes = "noteshere";
            //string IQTest = "Excellent";
            //string IQdate = "01/01/2022";
            //string IQnotes = "noteshere";
            //string lang1 = "arabic";
            //string lang1date = "01/01/2022";
            //string lang1notes = "noteshere";
            //string lang2 = "english";
            //string lang2date = "01/01/2022";
            //string lang2notes = "noteshere";
            //string lang3 = "Germany";
            //string lang3date = "01/01/2022";
            //string lang3notes = "noteshere";

            //string UserName = firstName + " " + FatherTitle;











            PdfReader reader1 = new PdfReader(oldFile);
            using (MemoryStream ms = new MemoryStream())
            {

                // Creating iTextSharp.text.pdf.PdfStamper object to write Data from iTextSharp.text.pdf.PdfReader object to FileStream object
                using (PdfStamper stamper = new PdfStamper(reader1, ms, '\0', true))
                {
                    // Getting total number of pages of the Existing Document
                    //int pageCount = reader1.NumberOfPages;

                    // Create New Layer for Watermark
                    PdfLayer layer = new PdfLayer("Layer", stamper.Writer);
                    // Loop through each Page

                    // Getting the Page Size
                    iTextSharp.text.Rectangle rect = reader1.GetPageSize(1);

                    // Get the ContentByte object
                    PdfContentByte cb = stamper.GetOverContent(1);

                    // Tell the cb that the next commands should be "bound" to this new layer
                    cb.BeginLayer(layer);

                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    BaseFont bfb = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    cb.SetColorFill(BaseColor.BLACK);
                    cb.SetFontAndSize(bf, 20);

                    cb.BeginText();

                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, userdata.Name, rect.Width - 360, rect.Height - 230, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, userdata.parent.FatherTitle, rect.Width - 360, rect.Height - 260, 0);

                    cb.SetFontAndSize(bf, 10);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, userdata.SchoolName, rect.Width - 270, rect.Height - 310, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, schoolsystem, rect.Width - 270, rect.Height - 332, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, userdata.Phone, rect.Width - 570, rect.Height - 346, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, userdata.parent.Address, rect.Width - 570, rect.Height - 378, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, userdata.parent.FatherEmail, rect.Width - 570, rect.Height - 410, 0);


                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, schoolreport, rect.Width - 354, rect.Height - 366, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Date: " + reportdate, rect.Width - 230, rect.Height - 366, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Note: " + reportnotes, rect.Width - 100, rect.Height - 366, 0);


                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, IQTestList.Name, rect.Width - 354, rect.Height - 399, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Date: " + IQTestList.Date.ToString("dd/MM/yyyy"), rect.Width - 230, rect.Height - 399, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Note: " + IQTestList.Note, rect.Width - 100, rect.Height - 399, 0);
                    var x = 399;
                    x = x + 24;
                    foreach (var item in LanguageList)
                    {
                        x = x + 10;
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, item.Name, rect.Width - 354, rect.Height - x, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Date: " + item.Date.ToString("dd/MM/yyyy"), rect.Width - 230, rect.Height - x, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Note: " + item.Note, rect.Width - 100, rect.Height - x, 0);

                    }


                    cb.SetFontAndSize(bfb, 10);
                    x = x + 20;
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "EXPERIENCE", rect.Width - 354, rect.Height - x, 0);
                    x = x + 10;


                    cb.SetLineWidth(1.5f);
                    cb.MoveTo(rect.Width - 354, rect.Height - x);
                    cb.LineTo(rect.Width - 35, rect.Height - x);
                    cb.SetRGBColorStroke(148, 182, 210);
                    cb.Stroke();
                    x = x + 20;
                    cb.SetFontAndSize(bfb, 8);

                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "VOLUNTEER WORK", rect.Width - 354, rect.Height - x, 0);
                    // Ibrahim Solve this if file not found
                    if (userdata.ImageURL != null)
                    {
                        iTextSharp.text.Image StudentImage = iTextSharp.text.Image.GetInstance(new Uri( env.WebRootPath +"\\"+ userdata.ImageURL));
                        StudentImage.ScaleAbsolute(15, 15);
                        #region circle
                        StudentImage.ScaleToFit(30, 30);
                        float centerX = 30;
                        float centerY = 30;
                        float radius = 15;
                        cb.SaveState();
                        cb.Circle(centerX, centerY, radius);
                        cb.RestoreState();
                        #endregion
                        cb.AddImage(StudentImage, 165f, 0, 0, 165f, 45, 530);

                    }
                    cb.SetFontAndSize(bf, 10);
                    x = x + 15;

                    foreach (var item in VolunteerList)
                    {

                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, item.Name, rect.Width - 354, rect.Height - x, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Date: " + item.Date.ToString("dd/MM/yyyy"), rect.Width - 230, rect.Height - x, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Note: " + item.Note, rect.Width - 100, rect.Height - x, 0);
                        x = x + 10;
                    }
                    x = x + 14;
                    cb.SetFontAndSize(bfb, 8);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INTERNSHIPS", rect.Width - 354, rect.Height - x, 0);
                    cb.SetFontAndSize(bf, 10);
                    x = x + 15;

                    foreach (var item in InternshipsList)
                    {

                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, item.Course.Name, rect.Width - 354, rect.Height - x, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Date: " + item.CertificationDate.ToString("dd/MM/yyyy"), rect.Width - 230, rect.Height - x, 0);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Note: " + item.Status, rect.Width - 100, rect.Height - x, 0);
                        x = x + 10;
                    }
                    cb.EndText();

                    // Close the layer
                    cb.EndLayer();

                }
                return (ms.ToArray(), $"{UserName}_CV.pdf");
            }

        }
    }
}

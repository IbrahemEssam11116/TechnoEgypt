using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;

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
        public (byte[],string) CreateCertificate(int userc)
        {

            var usercoursedata = _dBContext.childCourses.Where(w => w.Id == userc).Include(w => w.Course).Include(w => w.Child).ThenInclude(w => w.parent).FirstOrDefault();
            if(usercoursedata == null)
            {
                return (null,null);
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
                Rectangle rect = reader1.GetPageSize(1);

                // Get the ContentByte object
                PdfContentByte cb = stamper.GetOverContent(1);

                // Tell the cb that the next commands should be "bound" to this new layer
                cb.BeginLayer(layer);

                BaseFont bfuser = BaseFont.CreateFont(@pathfontuser, "Identity-H", BaseFont.EMBEDDED);
                cb.SetFontAndSize(bfuser, 20);
                cb.SetRGBColorFill(6,145,207);
                cb.BeginText();

                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, UserName, rect.Width - 620, rect.Height - 360, 0);
                BaseFont bfcourse = BaseFont.CreateFont(@pathfontcourse, "Identity-H", BaseFont.EMBEDDED);
                cb.SetFontAndSize(bfcourse, 20);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CourseName, rect.Width - 620, rect.Height - 465, 0);
                cb.SetFontAndSize(bfuser, 10);
                cb.SetRGBColorFill(35,31,32);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Cdate.ToShortDateString(), rect.Width - 740, rect.Height - 570, 0);
                cb.EndText();

                // Close the layer
                cb.EndLayer();

            }
            return (ms.ToArray(), $"{UserName}_{CourseName}.pdf");
            }

        }
    }
}

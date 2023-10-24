
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.IO;
using HotelCollection.Infrastructure.Interface;

namespace HotelCollection.Infrastructure.Services
{
    public class ConvertViewToPDF : IConvertViewToPDF
    {
        private IConverter _converter;
        private IViewRenderService _viewRender;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ISMTPService _emailSender;
        //private string projectRootFolder;

        public ConvertViewToPDF(IConverter converter, IViewRenderService viewRender, IHostingEnvironment env, ISMTPService emailSender)
        {
            _converter = converter;
            _viewRender = viewRender;
            _hostingEnvironment = env;
            _emailSender = emailSender;
        }
   

        public async Task<bool> CreatePDF(string emailMessage)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;

            var filePath = Path.Combine(webRootPath, "Uploads", "REQ_" +  ".pdf");
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle =  "REQUISITION DETAILS",
                Out = Path.Combine(webRootPath, "Uploads", "REQ_" +".pdf") //@"D:\PDFCreator\Employee_Report.pdf"
            };
            //Out = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TicketTemplate", ticketViewModel.TicketNo + ".pdf")) //@"D:\PDFCreator\Employee_Report.pdf"
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = "",//await _viewRender.RenderToStringAsync("/Views/PDFTemplates/RFQ_Notification.cshtml", rfqGenerationModel),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "css", "Ticket.css") }
            };
            //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
            //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            _converter.Convert(pdf);
            var subject = "RFQ NOTIFICATION";
            string emailBody = objectSettings.HtmlContent;// await _viewRender.RenderToStringAsync("/Views/PDFTemplates/PO.cshtml", rfqGenerationModel);
            await _emailSender.SendEmailAsync("", subject, emailMessage, filePath);


            return true;
        }

       
    }
}

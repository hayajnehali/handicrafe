using handicrafe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VisioForge.MediaFramework.Helpers;
using System.IO;

using IronPdf;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System.Net;


using System.Data;
using System.Data.OleDb;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace handicrafe.Controllers
{

    public class CustomerController :Controller
    {
        private ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public CustomerController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }



        public IActionResult Index(decimal id)
        {
            AboutU x = new AboutU();
            About about = new About();
            var idd = HttpContext.Session.GetInt32("Id");
            var account = _context.UserInfos.Where(a => a.Id == idd);
            ViewBag.account = account.ToList();

            var slider = _context.Homes.ToList();
            var product = _context.Handicrafts.Where(z => z.Quantity > 0).ToList();
            x = _context.AboutUs.FirstOrDefault(x => x.Id == 1);
            about = _context.Abouts.FirstOrDefault(x => x.Id == 1);
            var handicraft = _context.Handicrafts.Include(a => a.IdNavigation).Where(x => x.IdHandicraft == id).FirstOrDefault();


            var testimonil = _context.Testimonials.Include(p => p.IdNavigation).Where(z => z.Acceptt == "0").ToList();
            var data = Tuple.Create<IEnumerable<Home>, IEnumerable<Handicraft>, AboutU, About, IEnumerable<Testimonial>,Handicraft>
                (slider, product, x, about, testimonil,handicraft);

            return View(handicraft);
        }



        [HttpPost]
        public IActionResult Index(decimal ? IdHandicraft,int Quantity)
        {
            var idvisa = HttpContext.Session.GetInt32("Id");

            var visa = _context.VisaCards.Where(x=>x.Id==idvisa).Select(x=>x.IdVisa).FirstOrDefault();
           
            if(IdHandicraft == null)
            {
                return View("Firstpage", "Website");
            }
            Handicraft item= new Handicraft();
             item = _context.Handicrafts.Include(x=>x.IdNavigation).FirstOrDefault(z => z.IdHandicraft == IdHandicraft);
            var cost = item.Price;
            cost = cost * Quantity;
            item.Quantity = item.Quantity - Quantity;
            _context.Update(item);
            _context.SaveChanges();

            //minus the products cost from customer visa 
            VisaCard visaCard = new VisaCard();
            visaCard = _context.VisaCards.Include(h=>h.IdNavigation).FirstOrDefault(x => x.IdVisa == visa);
            if(visaCard == null)
            {
              return RedirectToAction("Firstpage","Website");
            }
            var zx = visaCard.Total;
            if (zx<cost)
            {
                ViewBag.enough = "The money in visa not enough";
                return View(item);
            }


            visaCard.Total = visaCard.Total - cost;
            _context.Update(visaCard);
            _context.SaveChanges();


            //%5 from cost
            double price= ((double)cost*5)/100;

            //add the products cost to customer visa 
            VisaCard visaCard1 = new VisaCard();
            visaCard1 = _context.VisaCards.Include(c=>c.IdNavigation).FirstOrDefault(x => x.IdNavigation.Id == item.IdNavigation.Id);
            double xx = (double)visaCard1.Total;
            double co = (double)cost;
            visaCard1.Total = (decimal)(xx + (co- price));
            _context.Update(visaCard1);
            _context.SaveChanges();



            //add money to company account
            FinancialAccount financialAccount= new FinancialAccount();
            financialAccount= _context.FinancialAccounts.FirstOrDefault(x => x.Id==1);
            financialAccount.FinancialReturn = financialAccount.FinancialReturn +(decimal)price;
            _context.Update(financialAccount);
            _context.SaveChanges();




            ViewBag.pr = item.Price;
















            //send email with attachment==============================

            var html = @"
            <link href=""https://fonts.googleapis.com/css?family=Libre Barcode 128""rel = ""stylesheet"" >
          <p style = ""font-family: 'Libre Barcode 128', serif; font-size:30px;""> Hello Google Fonts</p>";
            html = html+ ViewBag.pr;
            // Instantiate Renderer
            var Renderer = new IronPdf.ChromePdfRenderer();
            using var cover = Renderer.RenderHtmlAsPdf("<h1>Receipt</h1> <br/>" +
               "Name :"+ visaCard.IdNavigation.FName +"<br/>"+
               "username: " + visaCard.IdNavigation.UserName + "<br/>" +
               "Product Name :"+ item.Name+"<br/>"+
               "Product price :" + item.Price+
               "Total Cost :" + cost+"<br/> <hr/>"+
               "We will send to you emill after product go out"
               );

            /* Main Document */
            //As we have a Cover Page, we're going to start the page numbers at 2.
            Renderer.RenderingOptions.FirstPageNumber = 0;

            Renderer.RenderingOptions.HtmlFooter = new IronPdf.HtmlHeaderFooter()
            {

                MaxHeight = 10, //millimeters
                HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
                DrawDividerLine = true
            };

            using PdfDocument Pdf = Renderer.RenderHtmlAsPdf(html);

            //Merging PDF document with Cover page
            using PdfDocument merge = IronPdf.PdfDocument.Merge(cover, Pdf);

            //PDF Settings
            merge.SecuritySettings.AllowUserCopyPasteContent = false;
            //merge.SecuritySettings.UserPassword = "sharable";
          
            string wwwRootPath = _webHostEnviroment.WebRootPath;
            string fileNamee = Guid.NewGuid().ToString() + "_" + visaCard1.IdNavigation.FName;


            merge.SaveAs(@"wwwroot/pdf/"+fileNamee+".pdf");





            string name = visaCard.IdNavigation.FName + " " + visaCard.IdNavigation.LName;
            var email = visaCard.IdNavigation.Email;
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress("nhaa59449@gmail.com", "ALI HAYAJNEH");// replace with valid value
            Msg.Subject = "Purchase";
            Msg.To.Add(email); //replace with correct values
            Msg.Body = "Welcome " + name + " to our site\r\nWe wish you good luck\r\nPurchase completed successfully";
            Msg.IsBodyHtml = false;
            Msg.Priority = MailPriority.High;


            string attachmentsPath = @"wwwroot\pdf\";


            if (Directory.Exists(attachmentsPath))
            {
                
                string attachmentsPathh = @"wwwroot\pdf\" + fileNamee + ".pdf";


                Attachment file = new Attachment(attachmentsPathh);
                Msg.Attachments.Add(file);
            }



            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("nhaa59449@gmail.com", "ndmdejcqerflbmsy");// replace with valid value
            smtp.EnableSsl = true;
            smtp.Timeout = 20000;

            smtp.Send(Msg);
            ViewBag.file = "Purchase completed successfully";









            //save iffo in sales tabel===========
            Sale sale = new Sale();
            sale.DateSale = DateTime.Today;
            sale.IdHandicraft = item.IdHandicraft;
            sale.Id = visaCard.IdNavigation.Id;
            sale.Quantity = Quantity;
            sale.PdfEmail = fileNamee + ".pdf";
            _context.Add(sale);
            _context.SaveChanges();
            return View(item);
        }


        public IActionResult tovendor()
        {
            return View();
        }

        public IActionResult transfare()
        {
            var id = HttpContext.Session.GetInt32("Id");
            UserInfo userInfo = new UserInfo();
            userInfo= _context.UserInfos.Where(y=>y.Id==id).First();
            userInfo.RoleName = "wating";
            
            _context.Update(userInfo);
            _context.SaveChanges();
            return RedirectToAction("Firstpage", "Website");
        }
    }


}

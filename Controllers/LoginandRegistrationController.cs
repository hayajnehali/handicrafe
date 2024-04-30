using GLib;
using handicrafe.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VisioForge.Libs.MediaFoundation.OPM;




using System;
using System.Collections.Generic;
 
using Microsoft.AspNetCore.Mvc.Rendering;
 
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;

namespace handicrafe.Controllers
{
    public class LoginandRegistrationController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public LoginandRegistrationController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        [HttpPost]
        public IActionResult Login(string ? username, string ? password)
        {
            var item = _context.UserInfos.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (item != null)
            {
                HttpContext.Session.SetString("username", item.UserName);

                switch (item.RoleName)
                {
                    case "user":
                        {

                            HttpContext.Session.SetInt32("Id", (int)item.Id);// Id:5
                            return RedirectToAction("Firstpage", "Website");
                        }
                    case "wating":
                        {

                            HttpContext.Session.SetInt32("Id", (int)item.Id);// Id:5
                            return RedirectToAction("Firstpage", "Website");
                        }
                    case "admin":
                        {
                            HttpContext.Session.SetInt32("Id", (int)item.Id);
                            return RedirectToAction("Sales", "DashAdmin");
                        }

                    case "vendor":
                        {
                            HttpContext.Session.SetInt32("Id", (int)item.Id);
                            return RedirectToAction("Firstpage", "Website");
                        }
                }
            }
            else
            {
                ViewBag.error = "Invalid username or password";
                return View("index");
            }
            return View();
        }
        public IActionResult Index()
        {
            
            return View();
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FName,LName,Email,UserName,Password,RoleName,RegisteringDate,Image,ImageFile")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
              
                var itemed = _context.UserInfos.SingleOrDefault(x=>x.UserName==userInfo.UserName);

                if (itemed == null)
                {
                    userInfo.RoleName = "user";
                    userInfo.RegisteringDate = DateAndTime.Now;
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + userInfo.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await userInfo.ImageFile.CopyToAsync(fileStream);
                    }
                    userInfo.Image = fileName;
                    _context.Add(userInfo);
                    await _context.SaveChangesAsync();

                    VisaCard visaCard = new VisaCard();
                    visaCard.Id = userInfo.Id;
                    visaCard.Total = 1000;
                    _context.Add(visaCard);
                    await _context.SaveChangesAsync();
                    return View("Index" );
                }
                else
                {
                    ViewBag.error_in_regstration = "The username is used ,please change it";
                
                    return View("Index");
                }

            }
            ViewBag.error_in_regstration = "There is mistake in your Info";
            return View("Index");
        }

    }
}

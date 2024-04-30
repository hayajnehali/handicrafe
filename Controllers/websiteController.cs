using handicrafe.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace handicrafe.Controllers
{
    public class WebsiteController : Controller
    {
        private ModelContext _context;
        public WebsiteController( ModelContext context)
        {
            _context = context;
        }
        public IActionResult Firstpage()
        { 
            AboutU x = new AboutU();
            About about = new About();
            var id = HttpContext.Session.GetInt32("Id");
            var account = _context.UserInfos.Where(a=>a.Id==id);
            ViewBag.account = account.ToList();

            var slider = _context.Homes.ToList();
            var product=_context.Handicrafts.Where(z=>z.Quantity>0).ToList();
            x = _context.AboutUs.FirstOrDefault(x => x.Id == 1);
            about=_context.Abouts.FirstOrDefault(x => x.Id == 1);
            var testimonil = _context.Testimonials.Include(p=>p.IdNavigation).Where(z=>z.Acceptt == "0").ToList();
            var data = Tuple.Create<IEnumerable<Home>,IEnumerable<Handicraft>,AboutU,About,IEnumerable<Testimonial>>
                (slider,product,x,about,testimonil);
            return View(data);
        }
    }
}

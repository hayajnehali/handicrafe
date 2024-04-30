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
using Pango;
using System.Data.SqlTypes;

namespace handicrafe.Controllers
{
    public class DashAdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public
        DashAdminController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }


        public IActionResult Home()
        {
            var home = _context.Homes.ToList();



            return View(home);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LoginandRegistration");
        }


        public async Task<IActionResult> Home_Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Homes.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }
            return View(home);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Home_Edit(decimal id, [Bind("Id,Text2,Imag,Hometext,ImageFile_home")] Home home)
        {
            if (id != home.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (home.ImageFile_home != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        home.ImageFile_home.FileName;
                        string path = Path.Combine(wwwRootPath + "/image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile_home.CopyToAsync(fileStream);
                        }
                        home.Imag = fileName;
                        _context.Update(home);
                    }
                    _context.Update(home);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Home_EditExists(home.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Home");
            }
            return View(home);
        }

        private bool Home_EditExists(decimal id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<UserInfo> info = new List<UserInfo>();

            var userInf = _context.UserInfos.ToList();
            var feedback = _context.FeedbackHandicrafts.ToList();
            var financial = _context.FinancialAccounts.ToList();
            var final = Tuple.Create<IEnumerable<FinancialAccount>,
                IEnumerable<UserInfo>>(financial, userInf);



            var numberOfCustomer = (from a in _context.UserInfos where a.RoleName == "user" select a).ToList();
            ViewBag.numberOfCustomer = numberOfCustomer.Count();


            var numberOfVendor = (from a in _context.UserInfos where a.RoleName == "vendor" select a).ToList()
           ; ViewBag.numberOfVendor = numberOfVendor.Count();


            List<UserInfo> profiel = new List<UserInfo>();
            var Id = HttpContext.Session.GetInt32("Id");
            profiel = (from a in _context.UserInfos where a.Id == Id select a).ToList();
            ViewBag.profiel = profiel;



            //var Profits = (from a in _context.FinancialAccounts select a).ToList();
            var Profits = _context.FinancialAccounts.Sum(x => x.FinancialReturn);
            var costs = _context.FinancialAccounts.Sum(x => x.Costs);
            var Pro = Profits - costs;
            ViewBag.Profits = Pro;

            ViewBag.target = costs;

            var sale = (from a in _context.Sales select a).ToList();
            ViewBag.sale = sale.Count();




            return View(final);

            object GetUserInfos()
            {
                return _context.UserInfos;
            }
        }


        public IActionResult Profile()
        {

            List<UserInfo> info = new List<UserInfo>();
            var Id = HttpContext.Session.GetInt32("Id");
            info = (from a in _context.UserInfos where a.Id == Id select a).ToList();
            ViewBag.Profits = info;
            return View();

        }

        public IActionResult Feedback()
        {
            //ViewBag.feedbak = (from a in _context.FeedbackHandicrafts select a).ToList();


            var modelContext = _context.FeedbackHandicrafts.Include(f => f.IdNavigation);
            return View(modelContext.ToList());


        }


        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfos.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FName,LName,Email,UserName,Password,RoleName,RegisteringDate,Image,ImageFile")] UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userInfo.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        userInfo.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userInfo.ImageFile.CopyToAsync(fileStream);
                        }
                        userInfo.Image = fileName;
                    }
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists((int)userInfo.Id))
                    {
                        return NotFound();
                    }

                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }

        private bool UserInfoExists(decimal id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Vendor_Request()
        {
            var vendor = (from a in _context.UserInfos where a.RoleName == "wating" select a);
            ViewBag.vendor = vendor;
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit_vendor(decimal ? Id)
        {
            UserInfo vendor = new UserInfo();
            vendor = (from a in _context.UserInfos where a.Id == Id select a).FirstOrDefault();
            vendor.RoleName = "vendor";
            _context.Update(vendor);
            _context.SaveChanges();
            string name =vendor.FName +" "+ vendor.LName;
            var email = vendor.Email;
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress("nhaa59449@gmail.com", "ALI HAYAJNEH");// replace with valid value
            Msg.Subject = "Contact";
            Msg.To.Add(email); //replace with correct values
            Msg.Body = "Welcome "+ name+" to our site\r\nWe wish you good luck\r\nYour account has been activated";
            Msg.IsBodyHtml = false;
            Msg.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("nhaa59449@gmail.com", "ndmdejcqerflbmsy");// replace with valid value
            smtp.EnableSsl = true;
            smtp.Timeout = 20000;

            smtp.Send(Msg);
            return RedirectToAction("Vendor_Request");


        }

        public IActionResult Sales()
        {


            var salee = _context.Sales.ToList();

            //ViewBag.sal = sale.GroupBy(x => x.DateSale.Year)
            //    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            var year = salee.Select(x => x.DateSale.Year).ToList();
            ViewBag.year = year;
            List<int> count = new List<int>();
            foreach (var item in year)
            {
                count.Add(salee.Where(x => x.DateSale.Year == item).Count());
            }
            ViewBag.count = count;

            List<UserInfo> info = new List<UserInfo>();

            var userInf = _context.UserInfos.ToList();
            var feedback = _context.FeedbackHandicrafts.ToList();
            var financial = _context.FinancialAccounts.ToList();
            var final = Tuple.Create<IEnumerable<FinancialAccount>,
                IEnumerable<UserInfo>>(financial, userInf);



            var numberOfCustomer = (from a in _context.UserInfos where a.RoleName == "user" select a).ToList();
            ViewBag.numberOfCustomer = numberOfCustomer.Count();


            var numberOfVendor = (from a in _context.UserInfos where a.RoleName == "vendor" select a).ToList()
           ; ViewBag.numberOfVendor = numberOfVendor.Count();


            List<UserInfo> profiel = new List<UserInfo>();
            var Id = HttpContext.Session.GetInt32("Id");
            profiel = (from a in _context.UserInfos where a.Id == Id select a).ToList();
            ViewBag.profiel = profiel;



            //var Profits = (from a in _context.FinancialAccounts select a).ToList();
            var Profits = _context.FinancialAccounts.Sum(x => x.FinancialReturn);
            var costs = _context.FinancialAccounts.Sum(x => x.Costs);
            var Pro = Profits - costs;
            ViewBag.Profits = Pro;

            ViewBag.target = costs;

            var sale = (from a in _context.Sales select a).ToList();
            ViewBag.sale = sale.Count();











            var x = _context.Sales.Include(p=>p.IdNavigation).Include(p=>p.IdHandicraftNavigation);
            return View(x.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sales(DateTime ? first_date,DateTime ? last_date)
        {

            var salee = _context.Sales.ToList();

            //ViewBag.sal = sale.GroupBy(x => x.DateSale.Year)
            //    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            var year = salee.Select(x => x.DateSale.Year).ToList();
            ViewBag.year = year;
            List<int> count = new List<int>();
            foreach (var item in year)
            {
                count.Add(salee.Where(x => x.DateSale.Year == item).Count());

                //var x= _context.Sales.Include(c=>c.IdNavigation).Include(c=>c.IdHandicraftNavigation)
                //    .Where(x=>x.DateSale.Year == item).Sum(x=>x.Quantity*x.IdHandicraftNavigation.Price);
                //count.Add((int)x);
            }
            ViewBag.count = count;

            List<UserInfo> info = new List<UserInfo>();

            var userInf = _context.UserInfos.ToList();
            var feedback = _context.FeedbackHandicrafts.ToList();
            var financial = _context.FinancialAccounts.ToList();

            var numberOfCustomer = (from a in _context.UserInfos where a.RoleName == "user" select a).ToList();
            ViewBag.numberOfCustomer = numberOfCustomer.Count();


            var numberOfVendor = (from a in _context.UserInfos where a.RoleName == "vendor" select a).ToList()
           ; ViewBag.numberOfVendor = numberOfVendor.Count();


            List<UserInfo> profiel = new List<UserInfo>();
            var Id = HttpContext.Session.GetInt32("Id");
            profiel = (from a in _context.UserInfos where a.Id == Id select a).ToList();
            ViewBag.profiel = profiel;



            //var Profits = (from a in _context.FinancialAccounts select a).ToList();
            var Profits = _context.FinancialAccounts.Sum(x => x.FinancialReturn);
            var costs = _context.FinancialAccounts.Sum(x => x.Costs);
            var Pro = Profits - costs;
            ViewBag.Profits = Pro;

            ViewBag.target = costs;

            var sale = (from a in _context.Sales select a).ToList();
            ViewBag.sale = sale.Count();









            var resultt = _context.Sales.Include(p => p.IdNavigation).Include(p => p.IdHandicraftNavigation);
            if (first_date==null && last_date == null)
            {
                return View(resultt.ToList());
            }
            else if (first_date!=null && last_date==null)
            {
                var result = resultt.Where(x => x.DateSale.Date == first_date);
                return View(result.ToList());
            }
            else if (first_date == null && last_date != null)
            {
                var result = resultt.Where(x => x.DateSale.Date == last_date);
                return View(result.ToList());
            }
            else 
            {

                var result = resultt.Where(x => x.DateSale.Date >= first_date && x.DateSale.Date <= last_date);
                return View(result.ToList());
            }
            
        }

        private Task<IActionResult> RedirectToAction(List<Sale> sales)
        {
            throw new NotImplementedException();
        }




        //about us ==============================
        public async Task<IActionResult> Aboutus()
        {
            return View(await _context.AboutUs.FirstOrDefaultAsync(x=>x.Id==1));
        }

        // GET: AboutUs/Edit/5
        public async Task<IActionResult> Edit_About(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutU = await _context.AboutUs.FindAsync(id);
            if (aboutU == null)
            {
                return NotFound();
            }
            return View(aboutU);
        }

        // POST: AboutUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_About(decimal id, [Bind("Id,Locations,Phone,Email,Facebock,Instagram,Twitter,LocationL,LocationY")] AboutU aboutU)
        {
            if (id != aboutU.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUExists(aboutU.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aboutU);
        }

        private bool AboutUExists(decimal id)
        {
            throw new NotImplementedException();
        }

        public IActionResult About()
        {
            var about= _context.Abouts.FirstOrDefault(a=>a.Id==1);
            return View(about);
        }
        public async Task<IActionResult> Edit_About2(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutU = await _context.Abouts.FindAsync(id);
            if (aboutU == null)
            {
                return NotFound();
            }
            return View(aboutU);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_About2(decimal id, [Bind("Id,Image,Text,ImageFile")] About about)
        {
            if (id != about.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (about.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        about.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await about.ImageFile.CopyToAsync(fileStream);
                        }
                        about.Image = fileName;

                        _context.Update(about);
                    }
                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!About2Exists(about.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(About));
            }
            return View(about);
        }

        private bool About2Exists(decimal id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Testimonial()
        {
            var testimonial = _context.Testimonials.Include(z=>z.IdNavigation).ToList();

            return View(testimonial);
        }

        public IActionResult EditTistimonialToView(decimal id)
        {
            Testimonial testimonial = new Testimonial();
            testimonial = _context.Testimonials.FirstOrDefault(x=>x.IdTestimonial==id);
            testimonial.Acceptt = "1";
            _context.Update(testimonial);
            _context.SaveChanges();
            return RedirectToAction("Testimonial");
        }
        
        public IActionResult EditTistimonialToUnView(decimal id)
        {
            Testimonial testimonial = new Testimonial();
            testimonial = _context.Testimonials.FirstOrDefault(x => x.IdTestimonial == id);
            testimonial.Acceptt = "0";
            _context.Update(testimonial);
            _context.SaveChanges();
            return RedirectToAction("Testimonial");
        }       
        
        
        public IActionResult DeleteTistimonial(decimal id)
        {
            Testimonial testimonial = new Testimonial();
            var Testimonials = _context.Testimonials.Find(id);
            _context.Testimonials.Remove(Testimonials);
             _context.SaveChanges();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Testimonial");
        }

    }
}

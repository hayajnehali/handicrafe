using handicrafe.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace handicrafe.Controllers
{
	public class VendorController : Controller
	{
		private ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public VendorController(ModelContext context, IWebHostEnvironment webHostEnviroment)
		{
			_context= context;
            _webHostEnviroment= webHostEnviroment;
		}
		public IActionResult Index(decimal id)
		{
			return View();
		}


	   	public IActionResult My_Product()
	    {
			var id = HttpContext.Session.GetInt32("Id");
			var xx = _context.Handicrafts.Include(p=>p.IdNavigation);

            ViewBag.img = _context.UserInfos.FirstOrDefault(s=>s.Id==id).Image;

            var seales = _context.Sales.ToList();
            var handicrafts = _context.Handicrafts.ToList();

            //var year = (from s in seales
            //            join hh in handicrafts on s.IdHandicraft equals hh.Id
            //            group hh by new {hh.Datee.Year} into g
            //            select new
            //            {
            //                date = g.Key.Year,
            //                totalCost = g
            //            }
            //            ); 

            var products = xx.Where(item=>item.Id== id).ToList();
            ViewBag.PIECES= _context.Sales.Include(p=>p.IdNavigation).Include(x=>x.IdHandicraftNavigation).Where(x=>x.IdHandicraftNavigation.Id==id).Count();

            var cost = _context.Sales.Include(p => p.IdNavigation).Include(x => x.IdHandicraftNavigation).Where(x => x.IdHandicraftNavigation.Id == id).Sum(x=>x.IdHandicraftNavigation.Price * x.Quantity);
            ViewBag.total = _context.VisaCards.Where(z=>z.Id==id).Select(x=>x.Total).FirstOrDefault();
            ViewBag.Empty = _context.Handicrafts.Include(x =>x.IdNavigation).Where(c=>c.IdNavigation.Id==id).Where(x => x.Quantity == 0).Count();
            return View(products);
		}
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .Include(h => h.IdNavigation)
                .FirstOrDefaultAsync(m => m.IdHandicraft == id);
            if (handicraft == null)
            {
                return NotFound();
            }
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;
            return View(handicraft);
        }

        // GET: Handicrafts/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.UserInfos, "Id", "Email");
            var id = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == id).Image;

            return View();
        }

        // POST: Handicrafts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHandicraft,ImageFile,Name,Datee,Price,Quantity,Id,Image")] Handicraft handicraft)
        {
            if (ModelState.IsValid)
            {
                if (handicraft.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    handicraft.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await handicraft.ImageFile.CopyToAsync(fileStream);
                    }
                    handicraft.Image = fileName;
                 
                    _context.Add(handicraft);
                }
             
                var id = HttpContext.Session.GetInt32("Id");
                handicraft.Datee= DateTime.Today;
                handicraft.Id = (int)id;
                _context.Add(handicraft);
                await _context.SaveChangesAsync();
                return RedirectToAction("My_Product");
            }
            ViewData["Id"] = new SelectList(_context.UserInfos, "Id", "Email", handicraft.Id);
            return View(handicraft);
        }

        // GET: Handicrafts/Edit/5
        public async Task<IActionResult> Editt(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts.FindAsync(id);
            if (handicraft == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.UserInfos, "Id", "Id", handicraft.Id);
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;

            return View(handicraft);
        }



        // POST: Handicrafts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editt(decimal id, [Bind("IdHandicraft,Name,Datee,Price,Quantity,Id,Image,ImageFile")] Handicraft handicraft,int Iduser)
        {


            if (id != handicraft.IdHandicraft)
            {
                return NotFound();

            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (handicraft.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        handicraft.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                             handicraft.ImageFile.CopyToAsync(fileStream);
                        }
                        handicraft.Image = fileName;
                        //_context.Update(handicraft);
                    }
                    handicraft.Id = Iduser;
                    _context.Update(handicraft);
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandicraftExists(handicraft.IdHandicraft))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("My_Product");
            }
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;

            //ViewData["Id"] = new SelectList(_context.UserInfos, "Id", "Email", handicraft.Id);
            return View(handicraft);
        }
        // GET: Handicrafts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .Include(h => h.IdNavigation)
                .FirstOrDefaultAsync(m => m.IdHandicraft == id);
            if (handicraft == null)
            {
                return NotFound();
            }
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;

            return View(handicraft);
        }

        // POST: Handicrafts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var handicraft = await _context.Handicrafts.FindAsync(id);
            _context.Handicrafts.Remove(handicraft);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandicraftExists(decimal id)
        {
            return _context.Handicrafts.Any(e => e.IdHandicraft == id);
        }

        public IActionResult Profile()
        {

            List<UserInfo> info = new List<UserInfo>();
            var Id = HttpContext.Session.GetInt32("Id");
            info = (from a in _context.UserInfos where a.Id == Id select a).ToList();
            ViewBag.Profits = info;
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;
            return View();

        }


        public async Task<IActionResult> Edit_profile(decimal? id)
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
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_profile(decimal id, [Bind("Id,FName,LName,Email,UserName,Password,RoleName,RegisteringDate,Image,ImageFile")] UserInfo userInfo)
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
                return RedirectToAction("Profile");
            }
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;
            return View(userInfo);
        }

        private bool UserInfoExists(int id)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Feedback(string message)
        {
            var id = HttpContext.Session.GetInt32("Id");
            FeedbackHandicraft feedback = new FeedbackHandicraft();
            feedback.Message=message;
            feedback.Id = (int)id;
            _context.Add(feedback);
            _context.SaveChanges();
            var idd = HttpContext.Session.GetInt32("Id");
            ViewBag.img = _context.UserInfos.FirstOrDefault(s => s.Id == idd).Image;
            return RedirectToAction("Firstpage", "Website");
        }
    }
}

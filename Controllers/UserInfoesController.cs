using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using handicrafe.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace handicrafe.Controllers
{
    public class UserInfoesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public UserInfoesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: UserInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserInfos.ToListAsync());
        }

        // GET: UserInfoes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FName,LName,Email,UserName,Password,RoleName,RegisteringDate,Image,ImageFile")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                var item=_context.UserInfos.SingleOrDefaultAsync(m => m.UserName == userInfo.UserName);
                if (item == null)
                {
                    userInfo.RoleName = "user";
                    userInfo.RegisteringDate = DateTime.Today;
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
                    return RedirectToAction("Index", "LoginandRegistration");
                }
                else
                {
                    ViewBag.error = "The username is used ,please change it";

                    ErrorViewModel s=new ErrorViewModel();
                    
                    s.RequestId = "The username is used ,please change it";
                   
                    return RedirectToAction("Index", "LoginandRegistration",s);
                }
                
            }
            ViewBag.error_in_regstration = "There is mistake in your Info";
            return RedirectToAction("Index", "LoginandRegistration");
        }

        // GET: UserInfoes/Edit/5
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
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FName,LName,Email,UserName,Password,RoleName,RegisteringDate,Image")] UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.Id))
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
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var userInfo = await _context.UserInfos.FindAsync(id);
            _context.UserInfos.Remove(userInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(decimal id)
        {
            return _context.UserInfos.Any(e => e.Id == id);
        }
    }
}

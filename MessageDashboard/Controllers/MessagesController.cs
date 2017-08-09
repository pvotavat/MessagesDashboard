using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dashboard.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        public MessagesController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        
        // GET: /Messages/
        // GET Messages for the logged in user
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return View(db.Messages.ToList().Where(messages => messages.User.Id == currentUser.Id));
        }

        // GET: /Messages/All
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> All()
        {
            return View(await db.Messages.ToListAsync());
        }

        // GET: /Messages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId()); 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = await db.Messages.FindAsync(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            if (messages.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(messages);
        }

        // GET: /Messages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Message,IsActive")] Messages messages)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId()); 
            if (ModelState.IsValid)
            {
                messages.User = currentUser;
                db.Messages.Add(messages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(messages);
        }

        // GET: /Messages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId()); 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = await db.Messages.FindAsync(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            if (messages.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(messages);
        }

        // POST: /Messages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Message,IsActive")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messages).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(messages);
        }

        // GET: /Messages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId()); 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = await db.Messages.FindAsync(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            if (messages.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            } 
            return View(messages);
        }

        // POST: /Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Messages messages = await db.Messages.FindAsync(id);
            db.Messages.Remove(messages);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

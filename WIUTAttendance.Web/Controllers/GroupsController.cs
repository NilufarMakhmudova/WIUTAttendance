using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WIUTAttendance.Services;

namespace WIUTAttendance.Web.Controllers
{
    
    public class GroupsController : Controller
    {
        private UnitOfWork unitOfWork = null;

         public GroupsController()
        : this(new UnitOfWork())
    {

    }

         public GroupsController(UnitOfWork uow)
    {
        this.unitOfWork = uow;
    }

        // GET: Groups
        public ActionResult Index()
        {
            return View();
        }

        // GET: Groups/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Groups/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Groups/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttendanceApplication.Models;

namespace AttendanceApplication.Controllers
{
    public class SeminarsController : Controller
    {
        private AttendanceEntities db = new AttendanceEntities();

        // GET: Seminars
        public ActionResult Index(string group, string time)
        {
            ViewBag.Time = db.Groups.Select(g => g.Time.AcademicYear + " " + g.Time.Period).Distinct();
           
            ViewBag.Group = db.Groups.Select(g => g.Name).Distinct();

            var groups = db.Groups.Include(g => g.Time).Include(g => g.Course);


            if (!String.IsNullOrEmpty(group) && !String.IsNullOrEmpty(time) )
            {
                int timeid = db.Times.Where(t => (t.AcademicYear + " " + t.Period).ToString() == time).Select(t => t.ID).First();
                groups = groups.Where(g =>
                               g.TimeID == timeid &&
                               g.Name == group );
            }
            else
            {
                if (!String.IsNullOrEmpty(group))
                            {
                                groups = groups.Where(g => g.Name == group);
                            }
                else
                {
                    if (!String.IsNullOrEmpty(time))
                    {
                        int timeid = db.Times.Where(t => (t.AcademicYear + " " + t.Period).ToString() == time).Select(t => t.ID).First();
                        groups = groups.Where(g => g.TimeID == timeid);
                    
                    }
                }
            }
    

            return View(groups.ToList());
        }

       //public ActionResult GroupTimetable(int? id)
       // {
       //     if (id == null)
       //     {
       //         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
       //     }

       //     Group group = db.Groups.Where(g => g.ID == id).First();
       //     if (group == null)
       //     {
       //         return HttpNotFound();
       //     }
       //     ViewBag.groupID = id;
       //     var seminars = db.Seminars.Include(s => s.Module).Include(s => s.Time).Include(s => s.Tutor).Where(s=>s.GroupID==group.ID);
       //     return View(seminars.ToList());
            
       // }


        // GET: Seminars/Details/5
        public ActionResult Details(int? seminarId)
        {
            if (seminarId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seminar seminar = db.Seminars.Find(seminarId);
            
            if (seminar == null )
            {
                return HttpNotFound();
            }
            return View(seminar);
        }

        // GET: Seminars/Create
        public ActionResult Create(int? groupId)
        {
            if (groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Group group = db.Groups.Find(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }

            var groupModules = db.GroupModules.Include(g => g.Module).Where(g => g.GroupID == groupId).Select(g => g.Module);

            var moduleTutors = new List<Tutor>();

           foreach (Module module in groupModules)
           {
               Tutor groupModuleTutor = db.ModuleTutors.Where(m => m.ModuleID == module.ID).Select(m => m.Tutor).First();
               moduleTutors.Add(groupModuleTutor);

           }
            var tutors = moduleTutors.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.Name + " " + t.Surname,
                Value = t.ID.ToString(),
            });

            ViewBag.ModuleID = new SelectList(groupModules, "ID", "Name");

            ViewBag.TutorID = tutors;

            PassListsToView();

            

            return View();
        }

        // POST: Seminars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ModuleID,TimeID,TutorID,Room")] Seminar seminar, int? groupID, string day, string building, string startTime )
        {
            if (!String.IsNullOrEmpty(day))

            {
                seminar.DayOfWeek = day;
            }

            if (!String.IsNullOrEmpty(building))
            {
                seminar.Building = building;
            }

            if (!String.IsNullOrEmpty(startTime))
            {
                seminar.StartTime = startTime;
            }

            if (groupID.HasValue)
            {
                //seminar.GroupID = (int)groupID;
            }

            if (ModelState.IsValid)
            {
                db.Seminars.Add(seminar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            ViewBag.TimeID = db.Times.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.AcademicYear + " " + t.Period,
                Value = t.ID.ToString(),
                Selected = seminar.TimeID == t.ID

            });
            var groupModules = db.GroupModules.Include(g => g.Module).Where(g => g.GroupID == groupID).Select(g => g.Module);

            var moduleTutors = new List<Tutor>();

            foreach (Module module in groupModules)
            {
                Tutor groupModuleTutor = db.ModuleTutors.Where(m => m.ModuleID == module.ID).Select(m => m.Tutor).First();
                moduleTutors.Add(groupModuleTutor);

            }
            var tutors = moduleTutors.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.Name + " " + t.Surname,
                Value = t.ID.ToString(),
            });

            ViewBag.ModuleID = new SelectList(groupModules, "ID", "Name");

            ViewBag.TutorID = tutors;

            PassListsToView();


            return View(seminar);
        }

        // GET: Seminars/Edit/5
        public ActionResult Edit(int? seminarId, int? groupId)
        {
            if (seminarId == null || groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seminar seminar = db.Seminars.Find(seminarId);
            Group group = db.Groups.Find(groupId);
            if (seminar == null || group == null)
            {
                return HttpNotFound();
            }
            //ViewBag.GroupID = new SelectList(db.Groups, "ID", "Name", seminar.GroupID);
            ViewBag.ModuleID = new SelectList(db.Modules, "ID", "Name", seminar.ModuleID);
            ViewBag.TimeID = db.Times.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.AcademicYear + " " + t.Period,
                Value = t.ID.ToString(),
                Selected = seminar.TimeID == t.ID

            });
            ViewBag.TutorID = db.Tutors.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.Name + " " + t.Surname,
                Value = t.ID.ToString(),
                Selected = seminar.TutorID == t.ID

            });
            PassListsToView();
            return View(seminar);
        }

        // POST: Seminars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,GroupID,ModuleID,TimeID,TutorID,Room")] Seminar seminar, string day, string building, string startTime)
        {
            if (!String.IsNullOrEmpty(day))
            {
                seminar.DayOfWeek = day;
            }

            if (!String.IsNullOrEmpty(building))
            {
                seminar.Building = building;
            }

            if (!String.IsNullOrEmpty(startTime))
            {
                seminar.StartTime = startTime;
            }
            if (ModelState.IsValid)
            {
                db.Entry(seminar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.GroupID = new SelectList(db.Groups, "ID", "Name", seminar.GroupID);
            ViewBag.ModuleID = new SelectList(db.Modules, "ID", "Name", seminar.ModuleID);
            ViewBag.TimeID = db.Times.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.AcademicYear + " " + t.Period,
                Value = t.ID.ToString(),
                Selected = seminar.TimeID == t.ID

            });
            ViewBag.TutorID = db.Tutors.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.Name + " " + t.Surname,
                Value = t.ID.ToString(),
                Selected = seminar.TutorID == t.ID

            }); 
            PassListsToView();
            return View(seminar);
        }

        // GET: Seminars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seminar seminar = db.Seminars.Find(id);
            if (seminar == null)
            {
                return HttpNotFound();
            }
            return View(seminar);
        }

        // POST: Seminars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seminar seminar = db.Seminars.Find(id);
            db.Seminars.Remove(seminar);
            db.SaveChanges();
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


        public List<SelectListItem> returnDays()
        {
            List<SelectListItem> days = new List<SelectListItem>();
            days.Add(new SelectListItem { Text = "Monday", Value = "Monday", Selected = true });
            days.Add(new SelectListItem { Text = "Tuesday", Value = "Tuesday" });
            days.Add(new SelectListItem { Text = "Wednesday", Value = "Wednesday" });
            days.Add(new SelectListItem { Text = "Thursday", Value = "Thursday" });
            days.Add(new SelectListItem { Text = "Friday", Value = "Friday" });
            days.Add(new SelectListItem { Text = "Saturday", Value = "Saturday" });

            return days;
        
        }

        public List<SelectListItem> returnBuildings()
        {
            List<SelectListItem> buildings = new List<SelectListItem>();
            buildings.Add(new SelectListItem { Text = "Istiqlol", Value = "Istiqlol", Selected = true });
            buildings.Add(new SelectListItem { Text = "Amir Temur", Value = "Amir Temur" });
            buildings.Add(new SelectListItem { Text = "Lyceum", Value = "Lyceum" });

            return buildings;

        }

        public List<SelectListItem> returnStartTimes()
        {
            List<SelectListItem> startTimes = new List<SelectListItem>();
            startTimes.Add(new SelectListItem { Text = "9:00", Value = "9:00", Selected = true });
            startTimes.Add(new SelectListItem { Text = "10:00", Value = "10:00" });
            startTimes.Add(new SelectListItem { Text = "11:00", Value = "11:00" });
            startTimes.Add(new SelectListItem { Text = "12:00", Value = "12:00" });
            startTimes.Add(new SelectListItem { Text = "13:00", Value = "13:00" });
            startTimes.Add(new SelectListItem { Text = "14:00", Value = "14:00" });
            startTimes.Add(new SelectListItem { Text = "15:00", Value = "15:00" });
            startTimes.Add(new SelectListItem { Text = "16:00", Value = "16:00" });
            startTimes.Add(new SelectListItem { Text = "17:00", Value = "17:00" });
            startTimes.Add(new SelectListItem { Text = "18:00", Value = "18:00" });
            

            return startTimes;

        }

        public void PassListsToView()
        {
            ViewBag.days = returnDays();
            ViewBag.startTimes = returnStartTimes();
            ViewBag.buildings = returnBuildings();
        
        }

    }
}

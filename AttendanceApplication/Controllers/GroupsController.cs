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
    public class GroupsController : Controller
    {
        private AttendanceEntities db = new AttendanceEntities();

        // GET: Groups
        public ActionResult Index(string sortOrder, string time, string course, int? level)
        {

            ViewBag.GroupNameSortParm = string.IsNullOrEmpty(sortOrder) ? "gname" : "";
            ViewBag.LevelSortParm = sortOrder == "int" ? "level" : "int";
            ViewBag.YearSortParm = string.IsNullOrEmpty(sortOrder) ? "year" : "";
            ViewBag.YearSortParm = string.IsNullOrEmpty(sortOrder) ? "period" : "";
            ViewBag.CourseNameSortParm = string.IsNullOrEmpty(sortOrder) ? "cname" : "";


            //select distinct academic year names, period types
            ViewBag.Time = db.Groups.Select(g => g.Time.AcademicYear + " " + g.Time.Period).Distinct();
            ViewBag.Course = db.Groups.Select(g => g.Course.Name).Distinct();
            ViewBag.level = db.Groups.Select(g => g.Level).Distinct();

            var groups = db.Groups.Include(g => g.Time).Include(g => g.Course);

            
            if (!String.IsNullOrEmpty(course) && !String.IsNullOrEmpty(time) && level.HasValue)
            {
                int timeid = db.Times.Where(t => (t.AcademicYear + " " + t.Period).ToString() == time).Select(t => t.ID).First();
                groups = groups.Where(g =>
                               g.TimeID == timeid &&
                               g.Course.Name == course &&
                               g.Level == level);
            }
            else
            {
                if (!String.IsNullOrEmpty(course) && !String.IsNullOrEmpty(time))
                {
                    int timeid = db.Times.Where(t => (t.AcademicYear + " " + t.Period).ToString() == time).Select(t => t.ID).First();
                    groups = groups.Where(g =>
                                 g.TimeID == timeid &&
                                 g.Course.Name == course);
                }
                else
                {
                    if (!String.IsNullOrEmpty(course) && level.HasValue)
                    {
                        groups = groups.Where(g =>
                               g.Course.Name == course &&
                               g.Level == level);
                    }
                    else
                    {

                        if (!String.IsNullOrEmpty(time) && level.HasValue)
                        {
                            int timeid = db.Times.Where(t => (t.AcademicYear + " " + t.Period).ToString() == time).Select(t => t.ID).First();
                            groups = groups.Where(g =>
                               g.TimeID == timeid &&
                               g.Level == level);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(course))
                            {
                                groups = groups.Where(g => g.Course.Name == course);
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(time))
                                {
                                    int timeid = db.Times.Where(t => (t.AcademicYear + " " + t.Period).ToString() == time).Select(t => t.ID).First();
                                    groups = groups.Where(g => g.TimeID == timeid);
                                }
                                else
                                {
                                    if (level.HasValue)
                                    {
                                        groups = groups.Where(g => g.Level == level);
                                    }

                                }

                            }

                        }



                    }



                }
            }
                

                switch (sortOrder)
                {
                    case "gname":
                        groups = groups.OrderBy(g => g.Name);
                        break;
                    case "level":
                        groups = groups.OrderBy(g => g.Level);
                        break;
                    case "year":
                        groups = groups.OrderBy(g => g.Time.AcademicYear);
                        break;
                    case "period":
                        groups = groups.OrderBy(g => g.Time.Period);
                        break;
                    case "cname":
                        groups = groups.OrderBy(g => g.Course.Name);
                        break;

                }

                return View(groups.ToList());

            
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            var years = db.Times.Select(t => t.AcademicYear).Distinct().ToList();
            var periods = db.Times.Select(t => t.Period).Distinct().ToList();

            ViewBag.academYear = new SelectList(years, "AcademicYear");
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name");
            ViewBag.period = new SelectList(periods, "Period");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CourseID,Level")] Group group, string academYear, string period)
        {
            var years = db.Times.Select(t => t.AcademicYear).Distinct().ToList();
            var periods = db.Times.Select(t => t.Period).Distinct().ToList();
            ViewBag.academYear = new SelectList(years, "AcademicYear", academYear);
            //add selectlist for period
            ViewBag.period = new SelectList(years, "Period", period);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", group.CourseID);

            //find the id of the selected time from database and assign to new group
            group.TimeID = (from t in db.Times where t.AcademicYear.Equals(academYear) && t.Period.Equals(period) select t.ID).First();


            if (ModelState.IsValid)
            {

                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }


            ViewBag.TimeID = db.Times.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.AcademicYear + " " + t.Period,
                Value = t.ID.ToString(),
                Selected = group.TimeID == t.ID

            });
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", group.CourseID);
           

            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CourseID,Level")] Group group, string timeID)
        {

            ViewBag.TimeID = db.Times.AsEnumerable().Select(t => new SelectListItem
            {
                Text = t.AcademicYear + " " + t.Period,
                Value = t.ID.ToString(),
                Selected = group.TimeID == t.ID

            });
            
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", group.CourseID);
            int timeIDConverted = db.Times.Where(t => t.ID.ToString() == timeID).Select(t => t.ID).First();
            group.TimeID = timeIDConverted;

            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(group);
        }


            
        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                Group group = db.Groups.Find(id);
                db.Groups.Remove(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ManageStudents(int? id, 
            string module1, string module2, string module3, string module4, 
            List<Student> searchResults, List<Student> groupStudents,
            string action, int? studentID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            //needed for directing to edit page
            ViewBag.group = group;

            if (group == null)
            {
                return HttpNotFound();
            }

            ////identify all module registrations for the same academic year and period as the group
            //var moduleRegs = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).Include(m => m.Time).
            //    Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
            //    Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year");

            ////identify all student level progression for the same level as the group
            //var levelProgs = db.LevelProgressions.Include(l => l.Student).Include(l => l.Time).Where(l=>l.LevelNumber==group.Level);

            ////identify all course registrations for the same course and time period
            //var courseRegs = db.CourseRegistrations.Where(c=>c.CourseID==group.CourseID).
            //    Where(c=>c.StartTimeID==group.TimeID||c.EndTimeID==group.TimeID);

            var groupAllocation = db.GroupAllocations;
            var groupModules = db.GroupModules;

            //select modules that are delivered for the same course, level and period as the group
            ViewBag.modules = db.ModuleTypes.Include(m => m.Module).
                    Where(m => m.CourseID == group.CourseID
                    && m.Level == group.Level).Where(m => m.DeliveredPeriod == group.Time.Period
                    || m.DeliveredPeriod == "Year").
                    Select(m => m.Module.Name);

            if (!String.IsNullOrEmpty(module1) && !String.IsNullOrEmpty(module2) && !String.IsNullOrEmpty(module3) && !String.IsNullOrEmpty(module4))
            {

                // find the ids of each selected module in dropdownlist
                int module1ID = db.Modules.Where(m => m.Name == module1).Select(m => m.ID).First();
                int module2ID = db.Modules.Where(m => m.Name == module2).Select(m => m.ID).First();
                int module3ID = db.Modules.Where(m => m.Name == module3).Select(m => m.ID).First();
                int module4ID = db.Modules.Where(m => m.Name == module4).Select(m => m.ID).First();

                //to start a new view with the same selected ids
                ViewBag.module1 = module1;
                ViewBag.module2 = module2;
                ViewBag.module3 = module3;
                ViewBag.module4 = module4;

                //find all students who are registered to each module for the same semester or year as the group
                List<Student> students1 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module1ID).
                    Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();
                List<Student> students2 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module2ID).
                    Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();
                List<Student> students3 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module3ID).
                    Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();
                List<Student> students4 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module4ID).
                    Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();

                switch (action)
                {
                    case "Delete from group":
                        if (groupStudents!=null)
                        {
                            Student studentToBeRemoved = groupStudents.Find(s=>s.ID==studentID);
                            groupStudents.Remove(studentToBeRemoved);
                            ViewBag.students = groupStudents;
                            ViewBag.searchResults = searchResults;
                        }
                        break;
                    case "Add to group":
                        Student studentToBeAdded = searchResults.Find(s=> s.ID==studentID);
                        groupStudents.Add(studentToBeAdded);
                        searchResults.Remove(studentToBeAdded);
                        ViewBag.students = groupStudents;
                        ViewBag.searchResults = searchResults;
                        break;
                    case "Show registered students":
                       searchResults = new List<Student>();
                
                //find student records that exist in all for module registrations and make a list
                foreach (Student student in students1)
                {
                    if (students2.Exists(s => s.ID == student.ID))
                    {
                        if (students3.Exists(s => s.ID == student.ID))
                        {
                            if (students4.Exists(s => s.ID == student.ID))
                            {
                                searchResults.Add(student);
                            }
                        }

                    }
                }
             
                ViewBag.searchResults = searchResults;
                        break;

                    case "Save":

                        break;

                }

                

            }
            
            return View(group);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageStudents([Bind(Include = "ID,Name,CourseID,Level")] Group group,
            string action, string module1, string module2, string module3, string module4, List<Student> searchResults, List<Student> groupStudents,
            int? studentID)
        {
            //select modules that are delivered for the same course, level and period as the group
            ViewBag.modules = db.ModuleTypes.Include(m => m.Module).
                    Where(m => m.CourseID == group.CourseID
                    && m.Level == group.Level).Where(m => m.DeliveredPeriod == group.Time.Period
                    || m.DeliveredPeriod == "Year").
                    Select(m => m.Module.Name);

            if (!String.IsNullOrEmpty(module1) && !String.IsNullOrEmpty(module2) && !String.IsNullOrEmpty(module3) && !String.IsNullOrEmpty(module4))
            {

                // find the ids of each selected module in dropdownlist
                int module1ID = db.Modules.Where(m => m.Name == module1).Select(m => m.ID).First();
                int module2ID = db.Modules.Where(m => m.Name == module2).Select(m => m.ID).First();
                int module3ID = db.Modules.Where(m => m.Name == module3).Select(m => m.ID).First();
                int module4ID = db.Modules.Where(m => m.Name == module4).Select(m => m.ID).First();

                //to start a new view with the same selected ids
                ViewBag.module1 = module1;
                ViewBag.module2 = module2;
                ViewBag.module3 = module3;
                ViewBag.module4 = module4;

                

                switch (action)
                {
                    case "Delete from group":
                        if (groupStudents != null)
                        {
                            Student studentToBeRemoved = groupStudents.Find(s => s.ID == studentID);
                            groupStudents.Remove(studentToBeRemoved);
                            ViewBag.students = groupStudents;
                            ViewBag.searchResults = searchResults;
                        }
                        break;
                    case "Add to group":
                        Student studentToBeAdded = searchResults.Find(s => s.ID == studentID);
                        groupStudents.Add(studentToBeAdded);
                        searchResults.Remove(studentToBeAdded);
                        ViewBag.students = groupStudents;
                        ViewBag.searchResults = searchResults;
                        break;
                    //case "Show registered students":
                    //    searchResults = new List<Student>();

                    //    //find student records that exist in all for module registrations and make a list
                    //    foreach (Student student in students1)
                    //    {
                    //        if (students2.Exists(s => s.ID == student.ID))
                    //        {
                    //            if (students3.Exists(s => s.ID == student.ID))
                    //            {
                    //                if (students4.Exists(s => s.ID == student.ID))
                    //                {
                    //                    searchResults.Add(student);
                    //                }
                    //            }

                    //        }
                    //    }

                    //    ViewBag.searchResults = searchResults;
                        
                    //    break;

                    case "Save":

                        break;

                }



            }

            return View();
        }

        [HttpPost]
        
        public ActionResult SearchStudents(string module1, string module2, string module3, string module4)
        {
            if (!String.IsNullOrEmpty(module1) && !String.IsNullOrEmpty(module2) && !String.IsNullOrEmpty(module3) && !String.IsNullOrEmpty(module4))
            {
                int module1ID = db.Modules.Where(m => m.Name == module1).Select(m => m.ID).First();
                int module2ID = db.Modules.Where(m => m.Name == module2).Select(m => m.ID).First();
                int module3ID = db.Modules.Where(m => m.Name == module3).Select(m => m.ID).First();
                int module4ID = db.Modules.Where(m => m.Name == module4).Select(m => m.ID).First();
                var searchResults = new List<Student>();
                //find all students who are registered to each module for the same semester or year as the group
                List<Student> students1 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module1ID).Where(m => m.TimeID == 6).
                    //Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    //Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();
                List<Student> students2 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module2ID).Where(m => m.TimeID == 6).
                    //Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    //Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();
                List<Student> students3 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module3ID).Where(m => m.TimeID == 6).
                    //Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    //Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();
                List<Student> students4 = db.ModuleRegistrations.Include(m => m.Student).Include(m => m.Module).
                    Where(m => m.ModuleID == module4ID).Where(m => m.TimeID == 6).
                    //Where(m => m.Time.AcademicYear == group.Time.AcademicYear).
                    //Where(m => m.Time.Period == group.Time.Period || m.Time.Period == "Year").
                    Select(m => m.Student).ToList();


                //find student records that exist in all for module registrations and make a list
                foreach (Student student in students1)
                {
                    if (students2.Exists(s => s.ID == student.ID))
                    {
                        if (students3.Exists(s => s.ID == student.ID))
                        {
                            if (students4.Exists(s => s.ID == student.ID))
                            {
                                searchResults.Add(student);
                            }
                        }

                    }
                }

                ViewBag.searchResults = searchResults;

            }
            return View();
        }

       

           }
}

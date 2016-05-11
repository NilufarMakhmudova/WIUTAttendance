using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AttendanceApplication.Models;

namespace AttendanceApplication.API
{
    public class ModulesController : ApiController
    {
        private AttendanceEntities db = new AttendanceEntities();

        // GET: api/Modules
        public IQueryable<ModuleModel> GetModules(int TutorID, int TimeID)
        {
            var moduleTutors = db.ModuleTutors.Include(m => m.Module).Where(m=>m.TutorID==TutorID && m.TimeID==TimeID);
            var modules = new List<ModuleModel>();
            foreach(var m in moduleTutors) {
            var moduleModel = new ModuleModel();
            moduleModel.ID = m.Module.ID;
            moduleModel.Name = m.Module.Name;
            modules.Add(moduleModel);
            }
            return modules.AsQueryable();
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
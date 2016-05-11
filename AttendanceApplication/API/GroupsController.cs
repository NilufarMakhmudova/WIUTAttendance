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
    public class GroupsController : ApiController
    {
        private AttendanceEntities db = new AttendanceEntities();

        // GET: api/Modules
        public IQueryable<GroupModel> GetModules(int ModuleID, int TimeID)
        {
            var moduleGroups = db.GroupModules.Include(m => m.Group).Where(m => m.ModuelD == ModuleID && m.Group.TimeID == TimeID);
            var groups = new List<GroupModel>();
            foreach (var m in moduleGroups)
            {
                var groupModel = new GroupModel();
                groupModel.ID = m.Group.ID;
                groupModel.Name = m.Group.Name;
                groups.Add(groupModel);
            }
            return groups.AsQueryable();
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

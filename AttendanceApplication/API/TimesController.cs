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
    public class TimesController : ApiController
    {
        private AttendanceEntities db = new AttendanceEntities();

        // GET: api/Times
        public IQueryable<TimeModel> GetTimes()
        {
            var times = db.Times;
            var timesToSend = new List<TimeModel>();
            foreach (var time in times) {
                var timeToSend = new TimeModel();
                timeToSend.ID = time.ID;
                timeToSend.AcademicYear = time.AcademicYear;
                timeToSend.Period = time.Period;
                timeToSend.StartDate = time.StartDate;
                timeToSend.EndDate = time.EndDate;
                timesToSend.Add(timeToSend);
             }

            return timesToSend.AsQueryable();
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
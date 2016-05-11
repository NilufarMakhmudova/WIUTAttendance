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
    public class TutorsController : ApiController
    {
        private AttendanceEntities db = new AttendanceEntities();

        // GET: api/Tutors
        public IQueryable<TutorModel> GetTutors(string username, string password)
        {
            var tutor = db.Tutors.Where(t=>t.EmailPrefix==username && t.Password==password).First();
            var result = new List<TutorModel>();
            var tutorModel = new TutorModel();
            tutorModel.ID= tutor.ID;
            result.Add(tutorModel);
            return result.AsQueryable();
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
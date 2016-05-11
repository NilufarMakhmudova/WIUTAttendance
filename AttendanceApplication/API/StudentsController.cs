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
    public class StudentsController : ApiController
    {
        private AttendanceEntities db = new AttendanceEntities();
        // GET: api/Students
        public IQueryable<StudentModel> GetStudents(int ModuleID, int TimeID)
        {

            var moduleRegistrations = db.ModuleRegistrations.Where(m => m.ModuleID == ModuleID && m.TimeID == TimeID).Include(m => m.Student);
            var registeredStudents = new List<StudentModel>();
            foreach (var m in moduleRegistrations)
            {
                var student = m.Student;
                var studentModel = new StudentModel();
                studentModel.StudentID = student.StudentID;
                studentModel.Name = student.Name;
                studentModel.Surname = student.Surname;
                studentModel.EmailPrefix = student.EmailPrefix;
                registeredStudents.Add(studentModel);
            }

            return registeredStudents.AsQueryable();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.ID == id) > 0;
        }
    }
}
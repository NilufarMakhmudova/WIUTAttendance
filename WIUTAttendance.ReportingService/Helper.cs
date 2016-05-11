using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using WIUTAttendance.DataAccess;

namespace WIUTAttendance.ReportingServices
{
    public static class Helper
    {
       
        public static List<Student> GetStudentsWithAttendanceOffence() {
            WIUTAttendanceEntities db = new WIUTAttendanceEntities();
            var students = db.Students;
            return students.ToList();
        }
    }
}

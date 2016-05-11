using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIUTAttendance.DataAccess;

namespace WIUTAttendance.ViewModels
{
    public class StudentsViewModel
    {
        public int groupID { get; set; }
        public string groupName { get; set; }
        public string groupAcademicYear { get; set; }
        public string groupSemester { get; set; }
        public string groupCourse { get; set; }

        public List<StudentModulesViewModel> AllocatedStudents { get; set; }
        public List<StudentModulesViewModel> CandidateStudents { get; set; }

    }
}

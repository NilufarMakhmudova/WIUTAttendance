//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WIUTAttendance.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Group
    {
        public Group()
        {
            this.GroupAllocations = new HashSet<GroupAllocation>();
            this.GroupModules = new HashSet<GroupModule>();
            this.Seminars = new HashSet<Seminar>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int TimeID { get; set; }
        public int CourseID { get; set; }
        public int Level { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual ICollection<GroupAllocation> GroupAllocations { get; set; }
        public virtual ICollection<GroupModule> GroupModules { get; set; }
        public virtual Time Time { get; set; }
        public virtual ICollection<Seminar> Seminars { get; set; }
    }
}

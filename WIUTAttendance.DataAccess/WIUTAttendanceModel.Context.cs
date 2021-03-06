﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WIUTAttendanceEntities : DbContext
    {
        public WIUTAttendanceEntities()
            : base("name=WIUTAttendanceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseRegistration> CourseRegistrations { get; set; }
        public virtual DbSet<GroupAllocation> GroupAllocations { get; set; }
        public virtual DbSet<GroupModule> GroupModules { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<LevelProgression> LevelProgressions { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleRegistration> ModuleRegistrations { get; set; }
        public virtual DbSet<ModuleTutor> ModuleTutors { get; set; }
        public virtual DbSet<ModuleType> ModuleTypes { get; set; }
        public virtual DbSet<Seminar> Seminars { get; set; }
        public virtual DbSet<SeminarReschedule> SeminarReschedules { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }
    }
}

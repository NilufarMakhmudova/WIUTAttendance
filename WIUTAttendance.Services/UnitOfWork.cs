using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIUTAttendance.DataAccess;

namespace WIUTAttendance.Services
{
    public class UnitOfWork
    {
        private WIUTAttendanceEntities entities = null;

        // This will be called from controller default constructor
        public UnitOfWork()
        {
            entities = new WIUTAttendanceEntities();
            GroupsRepository = new GroupsRepository(entities);
        }

        // This will be created from test project and passed on to the
        // controllers parameterized constructors
        public UnitOfWork(IGroupsRepository groupsRepo)
        {
            GroupsRepository = groupsRepo;
        }

        public IGroupsRepository GroupsRepository
        {
            get;
            private set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIUTAttendance.DataAccess;

namespace WIUTAttendance.Services
{
    public interface IGroupsRepository
    {
        List<Group> GetAllGroups();
        Group GetGroupById(int id);
        void AddGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);
        void Save();
    }
}

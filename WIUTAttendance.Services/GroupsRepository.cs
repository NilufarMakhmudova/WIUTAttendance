using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIUTAttendance.DataAccess;

namespace WIUTAttendance.Services
{
   public class GroupsRepository : IGroupsRepository
    {
       WIUTAttendanceEntities entities = null;

       public GroupsRepository(WIUTAttendanceEntities entities) {
           this.entities = entities;
       }

        public List<DataAccess.Group> GetAllGroups()
        {
            return entities.Groups.ToList();
        }

        public DataAccess.Group GetGroupById(int id)
        {
            return entities.Groups.SingleOrDefault(group => group.ID == id);
        }

        public void AddGroup(DataAccess.Group group)
        {
            entities.Groups.Add(group);
        }

        public void UpdateGroup(DataAccess.Group group)
        {
            entities.Groups.Attach(group);
            entities.Entry(group).State = EntityState.Modified;
        }

        public void DeleteGroup(DataAccess.Group group)
        {
            entities.Groups.Remove(group);
        }

        public void Save()
        {
            entities.SaveChanges();
        }
    }
}

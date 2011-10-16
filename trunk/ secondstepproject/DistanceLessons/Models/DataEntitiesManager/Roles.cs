using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {

        public void AddRole(Role newRole)
        {
            _db.AddToRoles(newRole);
            _db.SaveChanges();
        }

        public List<Role> GetRoleList()
        {
            return _db.Roles.ToList<Role>();
        }

        public Role GetRole(string roleName)
        {
            return _db.Roles.SingleOrDefault(x => x.Name == roleName);
        }

        public Role GetRole(Guid roleId)
        {
            return _db.Roles.SingleOrDefault(x => x.RoleId == roleId);
        }

        public Guid GetRoleId(string name)
        {
            return (_db.Roles.SingleOrDefault(x => x.Name == name)).RoleId;
        }
    }
}
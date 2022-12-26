using crudOpertionAPI.entityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudOpertionAPI.Repository
{
    internal interface IRepo
    {
        IEnumerable<user_details> GetUserDetails();
        user_details GetUserId(int id);
        void Insert(user_details user);
        void AddUserToRole(user_role role);
        void Updateuser(user_details user);
        user_details validUser(string email, string password);
        void Delete(int id);
        void Deleterole(int id);
        void save();
    }
}

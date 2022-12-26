using crudOpertionAPI.entityData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace crudOpertionAPI.Repository
{
    public class Repo : IRepo
    {
        private NewDemoEntities _context;
        public Repo(NewDemoEntities context)
        {
            this._context = context;
        }
        public IEnumerable<user_details> GetUserDetails()
        {
            return _context.user_details.ToList();
        }
        public user_details GetUserId(int id)
        {
            return _context.user_details.Find(id);
        }
        public void Insert(user_details user)
        {
            _context.user_details.Add(user);
        }
        public void AddUserToRole(user_role role)
        {
            _context.user_role.Add(role);
        }
        public void Updateuser(user_details user)
        {
           /* var pass = (from m in _context.user_details where m.user_id == user.user_id select m.password).FirstOrDefault();
            user_details data = new user_details
            {
                firstname = user.firstname,
                lastname = user.lastname,
                phonenumber = user.phonenumber,
                email = user.email,
                password = pass,
            };*/
            _context.Entry(user).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            user_details user = _context.user_details.Find(id);
            _context.user_details.Remove(user);
        }

        public void Deleterole(int id)
        {
            user_role role = _context.user_role.SingleOrDefault(m => m.user_id == id);
            _context.user_role.Remove(role);
        }
        public void save()
        {
            _context.SaveChanges();
        }
        public user_details validUser(string email, string password)
        {
            var validate = /*_vamps.user_details.Where(x => x.email.Equals(email) &&
                                                     x.password.Equals(password)).ToList();*/
                (from m in _context.user_details
                 where
                m.email == email && m.password == password
                 select m).FirstOrDefault();
            return validate;
        }
    }
}
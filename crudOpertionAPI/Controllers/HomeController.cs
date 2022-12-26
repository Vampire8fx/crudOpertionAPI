using crudOpertionAPI.entityData;
using crudOpertionAPI.Models;
using crudOpertionAPI.Repository;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace crudOpertionAPI.Controllers
{
    public class HomeController : Controller
    {
        private IRepo Repo;
        NewDemoEntities _context = new NewDemoEntities();
        public HomeController()
        {
            Repo = new Repo(new entityData.NewDemoEntities());
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            try
            {
                var data = Repo.GetUserDetails();
                var users = data.Select(x => new
                {
                    user_id = x.user_id,
                    firstname = x.firstname,
                    lastname = x.lastname,
                    phonenumber = x.phonenumber,
                    email = x.email
                });
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Add(UserModel model)
        {
            try
            {
                var check = _context.user_details.FirstOrDefault(x => x.email == model.email);
                if (check == null)
                {
                    user_details user = new user_details()
                    {
                        firstname = model.firstname,
                        lastname = model.lastname,
                        phonenumber = model.phonenumber,
                        email = model.email,
                        password = GetMD5(model.password),
                    };
                    Repo.Insert(user);
                    user_role role = new user_role();
                    int Id = _context.user_details
                             .OrderByDescending(x => x.user_id)
                             .Take(0)
                             .Select(x => x.user_id)
                             .FirstOrDefault();
                    role.user_id = Id;
                    role.role_master_id = _context.role_master.Where(x => x.role_type == "User").Select
                    (u => u.role_master_id).SingleOrDefault();
                    Repo.AddUserToRole(role);
                    _context.Configuration.ValidateOnSaveEnabled = false;
                    Repo.save();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Login(LoginModel model)
        {
            string email = model.email;
            string password = GetMD5(model.password);
            var result = Repo.validUser(email, password);
            if (result == null)
            {
                return Json("failed");
            }
            else
            {
                var roles = (from users in _context.user_details
                             join roll in _context.user_role on users.user_id
                             equals roll.user_id
                             join mrole in _context.role_master
                             on roll.role_master_id equals mrole.role_master_id
                             where users.email == model.email
                             select mrole.role_type).SingleOrDefault();
                string Role = Convert.ToString(roles);
                if (Role == "Admin")
                {
                    Session["Id"] = result.user_id;
                    Session["Email"] = result.email;
                    Session["Phone"] = result.phonenumber;
                    Session["FullName"] = result.firstname + " " + result.lastname;
                    Session["Role"] = Role;
                }
                else if (Role == "User")
                {

                    Session["FullName"] = result.firstname + " " + result.lastname;
                    Session["Phone"] = result.phonenumber;
                    Session["Email"] = result.email;
                    Session["Id"] = result.user_id;
                    Session["Role2"] = Role;
                   
                }
                else
                {
                    return Json("Rfailed");
                }
                return Json("success");
            }
        }
        public ActionResult editdata(int id)
        {
            if (id != 0)
            {
                var data = Repo.GetUserId(id);
                user_details user = new user_details()
                {
                    user_id = data.user_id,
                    firstname = data.firstname,
                    lastname = data.lastname,
                    phonenumber = data.phonenumber,
                    email = data.email,
                    password = data.password
                };
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("failed");
            }
        }
        [HttpPost]
        public ActionResult Edit(user_details user)
        {
            if (user.user_id != 0)
            {
                Repo.Updateuser(user);
                Repo.save();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("failed");
            }
        }
        public JsonResult delete(int id)
        {
            Repo.Delete(id);
            Repo.Deleterole(id);
            Repo.save();
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
        public JsonResult Logout()
        {
            Session.Clear();//remove session
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}
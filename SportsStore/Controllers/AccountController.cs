using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Models;
using SportStore.Domain.Concrete;
using SportsStore.HTMLHelper;
using SportStore.Domain.Entities;


namespace SportsStore.Controllers
{
  
    public class AccountController : Controller
    {
        IAuthProvider authprovider;


        public AccountController(IAuthProvider auth)
        {
            authprovider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new EFDbContext())
                    {
                        var getUser = (from s in context.useraccount where s.user_name == model.UserName select s).FirstOrDefault();
                        if (getUser != null)
                        {
                            var hashcode = getUser.password_salt;
                            // passwoird hasing process call helper class method
                            var encodingpasswordstring = HashHelper.EncodePassword(model.Password, hashcode);
                            var query = (from s in context.useraccount where (s.user_name == model.UserName) && s.password.Equals(encodingpasswordstring) select s).FirstOrDefault();
                            if (query != null)
                            {

                                if (query.rolenum == 1)
                                {
                                    return RedirectToAction("Index", "Admin");
                                }
                                else
                                {
                                    return RedirectToAction("List", "Product");
                                }
                                //redirecttoaction (detail s + id.tostring (), )
                                //return view ("../admin/index"); url not change in brower

                            }
                            ViewBag.Errormessage = "Invallid user name or password";
                            return View();


                        }

                        ViewBag.Errormessage = "Invallid user name or password";
                        return View();
                    }

                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = " Error!!! contact cms@antra.net";
                    return View();
                }
                //if (authprovider.Authenticate(model.UserName, model.Password))
                //{
                //    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Incorrect username or password");
                //    return View();

                //}
            }
            else
            {
                return View();
            }

        }

        public ActionResult signup()
        {
            return View();
        }



        [HttpPost]
        public ActionResult signup(signupviewmodel user)
        {
            if (ModelState.IsValid)
            {
                using (EFDbContext context = new EFDbContext())
                {
                    useraccounts chuser = context.useraccount.Where(m => m.user_name == user.user_name || m.email == user.email).Select(s => s).FirstOrDefault();




                    //(from s in context.useraccount where s.user_name == user.user_name || s.email == user.email select s).FirstOrDefault();
                    if (chuser == null)
                    {
                        var keynew = HashHelper.GeneratePassword(10);
                        var password = HashHelper.EncodePassword(user.password, keynew);
                        useraccounts useracc = new useraccounts();
                        useracc.user_name = user.user_name;
                        useracc.password = password;
                        useracc.email = user.email;
                        useracc.password_salt = keynew;
                        useracc.rolenum = (int?)user.rolenum;
                        context.useraccount.Add(useracc);
                        context.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Login");

                    }
                    ViewBag.ErrorMessage = "User Already Exist";
                    return View();
                }

            }
            else
            {
                return View();
            }

        }
        [HttpPost]
      
        public JsonResult checkusername(string username)
        {
            using (EFDbContext context = new EFDbContext())
            {

                var checkname = context.useraccount.Where(m => m.user_name == username).Select(s => s).FirstOrDefault();
                if (checkname==null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }

        }



        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
    }
}
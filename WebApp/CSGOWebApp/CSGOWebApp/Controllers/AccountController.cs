using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Web;
using Models;
using RepositoryPattern;

namespace CSGOWebApp.Controllers
{
    public class AccountController : Controller
    {
        UserRepository userRepository = new UserRepository(UserFactory.GetContext(ContextType.MSSQL));
        // GET: Account
        public ActionResult Index()
        {
            User user = new User();
            try
            {
                user = (User)Session["User"];
            }
            catch { }
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (Request.Form["LoginWithPass"] != null)
            {
                return RedirectToAction("LoginWithPass", user);
            }
            else if (Request.Form["RegisterWithPass"] != null)
            {
                return RedirectToAction("RegisterWithPass", user);
            }
            else
            {
                return View("Index", "Home");
            }
        }

        public ActionResult LoginWithPass(User user)
        {
            try
            {
                User LoggedInUser = userRepository.GetByID(userRepository.LoginWithPass(user));
                Session["User"] = LoggedInUser;
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("LoginFailed");
            }
        }

        public ActionResult RegisterWithPass(User user)
        {
            try
            {
                User RegisteredUser = userRepository.GetByID(userRepository.RegisterPass(user));
                Session["User"] = RegisteredUser;
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }




        [HttpPost]
        public ActionResult LoginSteam()
        {
            if (Request.Form["LoginWithSteam"] != null)
            {
                Session["OpenIDAction"] = 2;
            }
            else if (Request.Form["RegisterWithSteam"] != null)
            {
                Session["OpenIDAction"] = 1;
            }
            else if (Request.Form["LinkSteam"] != null)
            {
                Session["OpenIDAction"] = 3;
            }
            return RedirectToAction("OpenID");
        }


        public ActionResult ChangePassword(User user)
        {
            User sessionUser = (User)Session["User"];
            string password = userRepository.GetPassword(user);
            if (user.Password == password && password.Length > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ActionFailed", "Account");
            }
        }



        public ActionResult ActionFailed()
        {
            return View();
        }


        public ActionResult OpenID(string returnUrl)
        {
            var rp = new OpenIdRelyingParty();
            var request = rp.CreateRequest("http://steamcommunity.com/openid/", Realm.AutoDetect, new Uri(Request.Url, Url.Action("Authenticate")));
            if (request != null)
            {
                if (returnUrl != null)
                {
                    request.AddCallbackArguments("returnUrl", returnUrl);
                }

                return request.RedirectingResponse.AsActionResultMvc5();
            }

            return View();
        }

        public ActionResult Authenticate(string returnUrl)
        {
            var rp = new OpenIdRelyingParty();
            var response = rp.GetResponse();
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        // Make sure we have a user account for this guy.
                        string identifier = response.ClaimedIdentifier; // convert to string so LinqToSQL expression parsing works.

                        long steam64id = Convert.ToInt64(identifier.Substring(36));

                        User tempUser = new User(steam64id);

                        int OpenIDAction = (int)Session["OpenIDAction"];

                        Session.Remove("OpenIDAction");

                        switch (OpenIDAction)
                        {
                            //Register
                            case 1:
                                User user = userRepository.GetByID(userRepository.Registersteam(tempUser));

                                if (user != null)
                                {
                                    Session["User"] = user;

                                    return View("FinishRegistration", user);
                                }
                                else
                                {
                                    return RedirectToAction("Index");
                                }

                            //Login
                            case 2:
                                Session["User"] = userRepository.GetByID(userRepository.LoginWithSteam(tempUser));

                                return View("Index");

                            //Link
                            case 3:
                                User sessionUser = (User)Session["User"];
                                tempUser.ID = sessionUser.ID;
                                return RedirectToAction("UpdateLoginData", tempUser);
                        }

                        ModelState.AddModelError(string.Empty, "An error occurred during login.");
                        break;

                    default:
                        ModelState.AddModelError(string.Empty, "An error occurred during login.");
                        break;
                }
            }

            return this.View("OpenID");
        }
        


        public ActionResult LogOut()
        {
            Session.Remove("User");
            return RedirectToAction("Index");
        }



        public ActionResult UpdateLoginData(User user)
        {
            if (user.Username != null)
            {
                if (user.Password != null)
                {
                    userRepository.UpdateLoginData(user, "UsernamePassword");
                }
                else
                {
                    userRepository.UpdateLoginData(user, "Username");
                }
            }
            else if (user.Password != null)
            {
                userRepository.UpdateLoginData(user, "Password");
            }

            if(user.Steam64ID > 0)
            {
                userRepository.UpdateLoginData(user, "Steam64ID");
            }

            Session["User"] = userRepository.GetByID(user.ID);

            return RedirectToAction("Index");
        }
    }
}
using ContactS.BLL.DTO;
using ContactS.BLL.Interfaces;
using ContactS.WEB.Models;
using ContactS.WEB.Models.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContactS.WEB.Controllers
{
    [Authorize]
    [Culture]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index(int page = 1)
        {
            var users = UserService.ListUsers(new BLL.DTO.Filtres.UserFilter(), page);
            List<UserListItemModel> result = new List<UserListItemModel>();
            foreach(var item in users.ResultUsers)
            {
                result.Add(new UserListItemModel
                {
                    user = item,
                    IsFriend = UserService.AreUsersIsFriends(item,
                    UserService.GetUserById(User.Identity.GetUserId())) ? 1 : item.Id==User.Identity.GetUserId()? 0:-1,
                });
            }
            return View(result);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    Role = "user"
                };
                int operation = await UserService.Create(userDto);
                if (operation == 0)
                    return View("SuccessRegister");
                else
                    ModelState.AddModelError("","Попробуйте ещё раз");
            }
            return View(model);
        }

        public ActionResult Delete()
        {
            return View(UserService.GetUserById(User.Identity.GetUserId()));
        }

        [HttpPost]
        public ActionResult Delete(UserDTO account)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            UserService.DeleteUser(account.Id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit()
        {
            return View(UserService.GetUserById(User.Identity.GetUserId()));
        }

        [HttpPost]
        public ActionResult Edit(UserDTO account)
        {
            UserService.EditUser(account);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult AddToFriend(string Id)
        {
            var Url = Request.UrlReferrer.AbsolutePath;
            UserService.AddUsersToFriends(UserService.GetUserById(User.Identity.GetUserId()), 
                UserService.GetUserById(Id));
            return Redirect(Url);
        }


        public ActionResult ClientProfile()
        {
            return View(UserService.GetUserById(User.Identity.GetUserId()));
        }

        public ActionResult ClientProfile(string id)
        {
            return View(UserService.GetUserById(id));
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "somemail@mail.ru",
                Password = "ad46D_ewr3",
                Name = "Семен Семенович Горбунков",
                Address = "ул. Спортивная, д.30, кв.75",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }
        

        public ActionResult Search(SearchModel search, int page=1)
        {
            var users = UserService.ListUsers(new BLL.DTO.Filtres.UserFilter
            {
                Login = search.UserName,
                Name = search.Name,
                Address = search.Adress
            }, page);

            List<UserListItemModel> items = new List<UserListItemModel>();
            foreach (var item in users.ResultUsers)
            {
                items.Add(new UserListItemModel
                {
                    user = item,
                    IsFriend = UserService.AreUsersIsFriends(item,
                    UserService.GetUserById(User.Identity.GetUserId())) ? 1 : item.Id == User.Identity.GetUserId() ? 0 : -1,
                });
            }

            return View(new SearcModelList { SearchModel = search, Users = items });
        }
    }
}
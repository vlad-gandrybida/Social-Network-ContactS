using ContactS.BLL.DTO;
using ContactS.BLL.Interfaces;
using ContactS.WEB.Models;
using ContactS.WEB.Models.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
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
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Index(int page = 1)
        {
            return RedirectToAction("Search", new SearchModel());
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult CheckUsername(string username)
        {
            var result = UserService.AreUserExist(username);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { UserName = model.Login, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", Resources.Resource.WrongSingIn);
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
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    UserName = model.Login,
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    Role = "user"
                };
                int operation = await UserService.Create(userDto);
                if (operation == 1)
                {
                    ClaimsIdentity claim = await UserService.Authenticate(userDto);
                    if(claim!=null)
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                    }
                    return View("SuccessRegister");
                }
                else
                    ModelState.AddModelError("", Resources.Resource.TryAgain);
            }
            return View(model);
        }

        public ActionResult Delete(string id)
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
        public ActionResult Edit(UserViewModel user)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            UserService.EditUser(new UserDTO {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                UserName = user.UserName,
                Password = user.Password
            });
            return Redirect(Url);
        }

        public ActionResult AddToFriend(string Id)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            UserService.AddUsersToFriends(UserService.GetUserById(User.Identity.GetUserId()),
                UserService.GetUserById(Id));
            return Redirect(Url);
        }

        public ActionResult ClientProfile(string id)
        {
            UserDTO user = UserService.GetUserById(id);
            return View(new ClientProfileViewModel
            {
                userInfo = new UserViewModel {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    UserName = user.UserName
                },
                Relation = UserService.AreUsersIsFriends(user,
                    UserService.GetUserById(User.Identity.GetUserId())) ? 1 : user.Id == User.Identity.GetUserId() ? 0 : -1
            });
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "adminSemen",
                Password = "ad46D_ewr3",
                Name = "Семен Семенович Горбунков",
                Address = "ул. Спортивная, д.30, кв.75",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }


        public ActionResult Search(SearchModel search, int page = 1)
        {
            UserListDTO users = UserService.ListUsers(new BLL.DTO.Filtres.UserFilter
            {
                Login = search.UserName,
                Name = search.Name,
                Address = search.Adress
            }, page);

            List<UserListItemModel> items = new List<UserListItemModel>();
            foreach (UserDTO item in users.ResultUsers)
            {
                items.Add(new UserListItemModel
                {
                    user = item,
                    IsFriend = UserService.AreUsersIsFriends(item,
                    UserService.GetUserById(User.Identity.GetUserId())) ? 1 : item.Id == User.Identity.GetUserId() ? 0 : -1,
                });
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserList", items);
            }
            return View(new SearcModelList { SearchModel = search, Users = items });
        }

        public ActionResult RemoveFromFriends(string id)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            UserService.RemoveUsersFromFriends(UserService.GetUserById(id), UserService.GetUserById(User.Identity.GetUserId()));
            return Redirect(Url);
        }
    }
}
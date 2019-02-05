using ContactS.BLL.DTO;
using ContactS.BLL.Interfaces;
using ContactS.ENUM.Request;
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
    public class AccountController : MyController
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IRequestService RequestService => HttpContext.GetOwinContext().GetUserManager<IRequestService>();

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
        public async Task<JsonResult> CheckUsername(string Login)
        {
            var result = await UserService.AreUserExist(Login);
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

        public async Task<ActionResult> Delete(string id)
        {
            return View(await UserService.GetUserById(User.Identity.GetUserId()));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(UserDTO account)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            await UserService.DeleteUser(account.Id);
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Edit()
        {
            return View(await UserService.GetUserById(User.Identity.GetUserId()));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel user)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            await UserService.EditUser(new UserDTO {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                UserName = user.UserName,
                Password = user.Password
            });
            return Redirect(Url);
        }

        public async Task<ActionResult> AddToFriend(string Id)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            Task<UserDTO> sender = UserService.GetUserById(User.Identity.GetUserId());
            Task<UserDTO> reciever = UserService.GetUserById(Id);
            await Task.WhenAll(sender, reciever);
            await RequestService.SendRequest(new RequestDTO
            {
                Sender = sender.Result,
                Receiver = reciever.Result,
                Type = RequestType.Friendsip
            });
            return Redirect(Url);
        }

        public async Task<ActionResult> ConfirmRequest(int Id)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            RequestDTO request = await RequestService.GetRequestById(Id);
            if(request.Type == RequestType.Friendsip)
            {
                await UserService.AddUsersToFriends(request.Receiver, request.Sender);
            }
            request.Status = RequestStatus.Confirmed;
            await RequestService.ConfirmRequest(request);
            return Redirect(Url);
        }

        public async Task<ActionResult> CancelRequest(int Id)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            RequestDTO request = await RequestService.GetRequestById(Id);
            request.Status = RequestStatus.Canceled;
            await RequestService.ConfirmRequest(request);
            return Redirect(Url);
        }

        public async Task<ActionResult> ClientProfile(string id)
        {
            UserDTO user = await UserService.GetUserById(id);
            return View(new ClientProfileViewModel
            {
                userInfo = new UserViewModel {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    UserName = user.UserName
                },
                Relation = await UserService.AreUsersIsFriends(user,
                    await UserService.GetUserById(User.Identity.GetUserId())) ? 1 : user.Id == User.Identity.GetUserId() ? 0 : -1
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


        public async Task<ActionResult> Search(SearchModel search, int page = 1)
        {
            UserListDTO users = await UserService.ListUsers(new BLL.DTO.Filtres.UserFilter
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
                    IsFriend = await UserService.AreUsersIsFriends(item,
                    await UserService.GetUserById(User.Identity.GetUserId())) ? 1 : item.Id == User.Identity.GetUserId() ? 0 : -1,
                });
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserList", items);
            }
            return View(new SearcModelList { SearchModel = search, Users = items });
        }

        public async Task<ActionResult> RemoveFromFriends(string id)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            Task<UserDTO> user1 = UserService.GetUserById(User.Identity.GetUserId());
            Task<UserDTO> user2 = UserService.GetUserById(id);
            await Task.WhenAll(user1, user2);
            await UserService.RemoveUsersFromFriends(user1.Result, user2.Result);
            return Redirect(Url);
        }

        public async Task<ActionResult> Requests(int page = 1)
        {
            var receiver = await UserService.GetUserById(User.Identity.GetUserId());
            RequestListDTO items = await RequestService.ListRequests( new BLL.DTO.Filtres.RequestFilter
                {
                    Receiver = receiver
                }, page);
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_RequestsList", items.ResultRequests);
            }
            return View(items.ResultRequests);
        }
    }
}
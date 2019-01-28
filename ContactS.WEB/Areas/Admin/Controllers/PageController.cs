using ContactS.BLL.Interfaces;
using ContactS.WEB.Areas.Admin.Models;
using ContactS.WEB.Models.Filters;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ContactS.WEB.Areas.Admin.Controllers
{
    [Culture]
    [Authorize(Roles = "admin")]
    public class PageController : Controller
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();
        private IMessageService MessageService => HttpContext.GetOwinContext().GetUserManager<IMessageService>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Messages(int page = 1)
        {
            BLL.DTO.MessageListDTO ListMessages = MessageService.ListDialogMessages(new BLL.DTO.Filtres.MessageFilter(), page);
            List<AdminMessageViewModel> messages = new List<AdminMessageViewModel>();
            foreach (BLL.DTO.MessageDTO message in ListMessages.ResultMessages)
            {
                messages.Add(new AdminMessageViewModel
                {
                    Id = message.Id,
                    Content = message.Content
                });
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsMessage", messages);
            }
            return View(messages);
        }

        [HttpPost]
        public ActionResult Messages(AdminMessageViewModel message)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            BLL.DTO.MessageDTO edit = MessageService.GetMessageById(message.Id);
            edit.Content = message.Content;
            MessageService.EditMessage(edit);
            return Redirect(Url);
        }

        public ActionResult Users(int page = 1)
        {
            BLL.DTO.UserListDTO ListUsers = UserService.ListUsers(new BLL.DTO.Filtres.UserFilter(), page);
            List<AdminUsersViewModel> users = new List<AdminUsersViewModel>();
            foreach (BLL.DTO.UserDTO user in ListUsers.ResultUsers)
            {
                users.Add(new AdminUsersViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Role = user.Role
                });
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsUser", users);
            }
            return View(users);
        }

        [HttpPost]
        public ActionResult Users(AdminUsersViewModel user)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            BLL.DTO.UserDTO edit = UserService.GetUserById(user.Id);
            edit.Role = user.Role;
            UserService.EditUser(edit);
            return Redirect(Url);
        }
    }
}
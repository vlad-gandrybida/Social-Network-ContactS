using ContactS.BLL.Interfaces;
using ContactS.WEB.Areas.Admin.Models;
using ContactS.WEB.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContactS.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PageController : MyController
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();
        private IMessageService MessageService => HttpContext.GetOwinContext().GetUserManager<IMessageService>();

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Messages(int page = 1)
        {
            BLL.DTO.MessageListDTO ListMessages = await MessageService.ListDialogMessages(new BLL.DTO.Filtres.MessageFilter(), page);
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
        public async Task<ActionResult> Messages(AdminMessageViewModel message)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            BLL.DTO.MessageDTO edit = await MessageService.GetMessageById(message.Id);
            edit.Content = message.Content;
            await MessageService.EditMessage(edit);
            return Redirect(Url);
        }

        public async Task<ActionResult> Users(int page = 1)
        {
            BLL.DTO.UserListDTO ListUsers = await UserService.ListUsers(new BLL.DTO.Filtres.UserFilter(), page);
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
        public async Task<ActionResult> Users(AdminUsersViewModel user)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            BLL.DTO.UserDTO edit = await UserService.GetUserById(user.Id);
            edit.Role = user.Role;
            await UserService.EditUser(edit);
            return Redirect(Url);
        }
    }
}
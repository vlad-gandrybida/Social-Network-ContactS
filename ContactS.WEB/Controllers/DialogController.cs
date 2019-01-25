using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Interfaces;
using ContactS.WEB.Models;
using ContactS.WEB.Models.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace ContactS.WEB.Controllers
{
    [Authorize]
    [Culture]
    public class DialogController : Controller
    {
        private IDialogService DialogService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IDialogService>();
            }
        }

        private IMessageService MessageService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IMessageService>();
            }
        }

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        public ActionResult Index()
        {
            var usersDialogs = DialogService
                .ListDialogs(new DialogFilter
                {
                    Account = UserService
                    .GetUserById(User.Identity.GetUserId())
                }).ResultDialogs.ToList();
            DialogListModel result = new DialogListModel();
            foreach(var dialog in usersDialogs)
            {
                MessageDTO lastMessage = MessageService.ListDialogMessages(new MessageFilter
                {
                    Chat = dialog
                }).ResultMessages.OrderBy(x=>x.Time).LastOrDefault();
                string message;
                if (lastMessage != null)
                {
                    message = lastMessage.Content;
                    if (message.Length > 50)
                    {
                        message = message.Substring(0, 50) + "...";
                    }
                }
                else
                {
                    message = "Історія переписки пуста";
                }
                result.dialogs.Add(new DialogViewModel
                {
                    DialogId = dialog.Id,
                    Dialog = dialog.Name,
                    LastMessage = message,
                    Data = lastMessage != null ? lastMessage.Time.ToShortDateString()
                    + "  " + lastMessage.Time.ToShortTimeString() : " ",
                    Sender = lastMessage != null ? lastMessage.Sender.Name
                    : " "
                });
            }
            return View(result);
        }

        public ActionResult Create()
        {
            var friends = UserService.ListFriendsOfUser(UserService.GetUserById(User.Identity.GetUserId()));
            return View(friends);
        }
        
        [HttpPost]
        public async Task<ActionResult> Create(string id)
        {
            var creator = UserService.GetUserById(User.Identity.GetUserId());
            var reciver = UserService.GetUserById(id);
            var DialogId = await DialogService.CreateDialog(new DialogDTO {
                Users = new List<UserDTO> { creator, reciver},
                Name = $"{creator.Name} and {reciver.Name} dialog"
            });
            return RedirectToAction("OpenDialog", new { id = DialogId });
        }

        public ActionResult Delete(int id)
        {
            var Dialog = DialogService.GetDialogById(id);
            var user = UserService.GetUserById(User.Identity.GetUserId());
            if (!DialogService.GetUsersInDialog(Dialog).Contains(user)) return RedirectToAction("AccessDenied", "Page");

            return View(Dialog);
        }

        [HttpPost]
        public ActionResult Delete(DialogDTO Dialog)
        {
            DialogService.DeleteDialog(Dialog.Id);

            return RedirectToAction("Index");
        }

        public ActionResult OpenDialog(int id, int page = 1)
        {
            var dialog = DialogService.GetDialogById(id);
            var userList = DialogService.GetUsersInDialog(dialog);
            var user = UserService.GetUserById(User.Identity.GetUserId());
            if (userList.Find(x=>x.Id==user.Id)==null)
                return RedirectToAction("AccessDenied", "Page");

            var list = MessageService
                .ListDialogMessages(new MessageFilter { Chat = dialog }, page)
                .ResultMessages.ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MessageItems", list);
            }

            return View(new OpenDialogModel
            {
                Dialog = dialog,
                Messages = list,
                DialogId = dialog.Id,
                Page = page,
                Accounts = userList
            });
        }

        [HttpPost]
        public ActionResult OpenDialog(OpenDialogModel model)
        {
            var user = UserService.GetUserById(User.Identity.GetUserId());
            var dialog = DialogService.GetDialogById(model.DialogId);
            MessageService.PostMessageToDialog(dialog, user, model.NewMessage);
            return RedirectToAction("OpenDialog", new { id = dialog.Id });
        }

        public ActionResult DeleteMessage(int id)
        {
            var msg = MessageService.GetMessageById(id);
            if (msg.Sender.Id != User.Identity.GetUserId()) return RedirectToAction("AccessDenied", "Page");

            return View(new MessageModel { Message = msg, DialogId = msg.Dialog.Id });
        }

        [HttpPost]
        public ActionResult DeleteMessage(MessageModel model)
        {
            MessageService.DeleteMessage(model.Message);
            return RedirectToAction("OpenDialog", new { id = model.DialogId });
        }

        public ActionResult EditMessage(int id)
        {
            var msg = MessageService.GetMessageById(id);
            if (msg.Sender.Id != User.Identity.GetUserId()) return RedirectToAction("AccessDenied", "Page");

            return View(msg);
        }

        [HttpPost]
        public ActionResult EditMessage(MessageDTO model)
        {
            MessageService.EditMessage(model);
            return RedirectToAction("OpenDialog", new { id = model.Dialog.Id });
        }

        public ActionResult Edit(int id)
        {
            var Dialog = DialogService.GetDialogById(id);

            if (!DialogService.GetUsersInDialog(Dialog).Any(x=>x.Id== User.Identity.GetUserId())) return RedirectToAction("AccessDenied", "Page");

            return PartialView(Dialog);
        }

        [HttpPost]
        public ActionResult Edit(DialogDTO dialog)
        {
            DialogService.EditDialogName(dialog);
            return RedirectToAction("OpenDialog", new { id = dialog.Id });
        }

        public ActionResult RemovePeople(int id)
        {
            var dialog = DialogService.GetDialogById(id);
            var user = UserService.GetUserById(User.Identity.GetUserId());
            var userList = DialogService.GetUsersInDialog(dialog);
            if (!userList.Contains(user)) return RedirectToAction("AccessDenied", "Page");
            userList.Remove(user);
            var list = new List<SelectModel>();
            userList.ForEach(f => list.Add(new SelectModel { User= f }));
            return View(new RemovePeopleModel { Accounts = list, Dialog = dialog });
        }

        [HttpPost]
        public ActionResult RemovePeople(RemovePeopleModel model)
        {
            model.Accounts.Where(a => a.Invited).ForEach(a => DialogService.RemoveUserFromDialog(model.Dialog, a.User));

            return RedirectToAction("OpenDialog", "Dialog", new { id = model.Dialog.Id });
        }

        public ActionResult Leave(int id)
        {
            var Dialog = DialogService.GetDialogById(id);
            if (!DialogService.GetUsersInDialog(Dialog).Any(x => x.Id == User.Identity.GetUserId())) return RedirectToAction("AccessDenied", "Page");
            
            return PartialView(Dialog);
        }

        [HttpPost]
        public ActionResult Leave(DialogDTO Dialog)
        {
            var user = UserService.GetUserById(User.Identity.GetUserId());
            DialogService.RemoveUserFromDialog(Dialog, user);

            return RedirectToAction("Index");
        }
    }
}
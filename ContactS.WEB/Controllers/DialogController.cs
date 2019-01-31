using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Interfaces;
using ContactS.WEB.Models;
using ContactS.WEB.Models.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
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
        private IDialogService DialogService => HttpContext.GetOwinContext().GetUserManager<IDialogService>();

        private IMessageService MessageService => HttpContext.GetOwinContext().GetUserManager<IMessageService>();

        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        public ActionResult Index()
        {
            List<DialogDTO> usersDialogs = DialogService
                .ListDialogs(new DialogFilter
                {
                    Account = UserService
                    .GetUserById(User.Identity.GetUserId())
                }).ResultDialogs.ToList();
            DialogListModel result = new DialogListModel();
            foreach (DialogDTO dialog in usersDialogs)
            {
                MessageDTO lastMessage = MessageService.ListDialogMessages(new MessageFilter
                {
                    Chat = dialog
                }).ResultMessages.OrderBy(x => x.Time).LastOrDefault();
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
                    message = Resources.Resource.NoMessage;
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
            List<UserDTO> friends = UserService.ListFriendsOfUser(UserService.GetUserById(User.Identity.GetUserId()));
            return View(friends);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string id)
        {
            UserDTO creator = UserService.GetUserById(User.Identity.GetUserId());
            UserDTO reciver = UserService.GetUserById(id);
            int DialogId = await DialogService.CreateDialog(new DialogDTO
            {
                Users = new List<UserDTO> { creator, reciver },
                Name = $"{creator.Name} and {reciver.Name} dialog"
            });
            return RedirectToAction("OpenDialog", new { id = DialogId });
        }

        public ActionResult Delete(int id)
        {
            DialogDTO Dialog = DialogService.GetDialogById(id);
            UserDTO user = UserService.GetUserById(User.Identity.GetUserId());
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
            DialogDTO dialog = DialogService.GetDialogById(id);
            List<UserDTO> userList = DialogService.GetUsersInDialog(dialog);

            UserDTO user = UserService.GetUserById(User.Identity.GetUserId());
            if (userList.Find(x => x.Id == user.Id) == null)
                return RedirectToAction("AccessDenied", "Home");

            List<MessageDTO> list = MessageService
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
            UserDTO user = UserService.GetUserById(User.Identity.GetUserId());
            DialogDTO dialog = DialogService.GetDialogById(model.DialogId);
            MessageService.PostMessageToDialog(dialog, user, model.NewMessage);
            return RedirectToAction("OpenDialog", new { id = dialog.Id });
        }

        [HttpPost]
        public ActionResult DeleteMessage(MessageViewModel model)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            var message = MessageService.GetMessageById(model.Id);
            MessageService.DeleteMessage(message);
            return Redirect(Url);
        }

        [HttpPost]
        public ActionResult EditMessage(MessageViewModel model)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            var message = MessageService.GetMessageById(model.Id);
            message.Content = model.Content;
            MessageService.EditMessage(message);
            return Redirect(Url);
        }

        public ActionResult Edit(int id)
        {
            DialogDTO Dialog = DialogService.GetDialogById(id);

            if (!DialogService.GetUsersInDialog(Dialog).Any(x => x.Id == User.Identity.GetUserId())) return RedirectToAction("AccessDenied", "Home");

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
            DialogDTO dialog = DialogService.GetDialogById(id);
            UserDTO user = UserService.GetUserById(User.Identity.GetUserId());
            List<UserDTO> userList = DialogService.GetUsersInDialog(dialog);
            if (!userList.Contains(user)) return RedirectToAction("AccessDenied", "Home");
            userList.Remove(user);
            List<SelectModel> list = new List<SelectModel>();
            userList.ForEach(f => list.Add(new SelectModel { User = f }));
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
            DialogDTO Dialog = DialogService.GetDialogById(id);
            if (!DialogService.GetUsersInDialog(Dialog).Any(x => x.Id == User.Identity.GetUserId())) return RedirectToAction("AccessDenied", "Home");

            return PartialView(Dialog);
        }

        [HttpPost]
        public ActionResult Leave(DialogDTO Dialog)
        {
            UserDTO user = UserService.GetUserById(User.Identity.GetUserId());
            DialogService.RemoveUserFromDialog(Dialog, user);

            return RedirectToAction("Index");
        }
    }
}
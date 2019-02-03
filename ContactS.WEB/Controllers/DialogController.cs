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
    public class DialogController : MyController
    {
        private IDialogService DialogService => HttpContext.GetOwinContext().GetUserManager<IDialogService>();

        private IMessageService MessageService => HttpContext.GetOwinContext().GetUserManager<IMessageService>();

        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        public async Task<ActionResult> Index()
        {
            UserDTO user = await UserService
                    .GetUserById(User.Identity.GetUserId());
            List<DialogDTO> usersDialogs = (await DialogService
                .ListDialogs(new DialogFilter
                {
                    Account = user,
                })).ResultDialogs.ToList();
            DialogListModel result = new DialogListModel();
            foreach (DialogDTO dialog in usersDialogs)
            {
                MessageDTO lastMessage = (await MessageService.ListDialogMessages(new MessageFilter
                {
                    Chat = dialog
                })).ResultMessages.OrderBy(x => x.Time).LastOrDefault();
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

        public async Task<ActionResult> Create()
        {
            List<UserDTO> friends = await UserService.ListFriendsOfUser(await UserService.GetUserById(User.Identity.GetUserId()));
            return View(friends);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string id)
        {
            Task<UserDTO> creator = UserService.GetUserById(User.Identity.GetUserId());
            Task<UserDTO> reciver = UserService.GetUserById(id);
            await Task.WhenAll(creator, reciver);
            int DialogId = await DialogService.CreateDialog(new DialogDTO
            {
                Users = new List<UserDTO> { creator.Result, reciver.Result },
                Name = $"{creator.Result.Name} and {reciver.Result.Name} dialog"
            });
            return RedirectToAction("OpenDialog", new { id = DialogId });
        }

        public async Task<ActionResult> Delete(int id)
        {
            Task<UserDTO> user = UserService.GetUserById(User.Identity.GetUserId());
            Task<DialogDTO> Dialog = DialogService.GetDialogById(id);
            await Task.WhenAll(user, Dialog);
            if (!(await DialogService.GetUsersInDialog(Dialog.Result)).Contains(user.Result)) return RedirectToAction("AccessDenied", "Page");

            return View(Dialog);
        }

        [HttpPost]
        public async  Task<ActionResult> Delete(DialogDTO Dialog)
        {
            await DialogService.DeleteDialog(Dialog.Id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> OpenDialog(int id, int page = 1)
        {
            DialogDTO dialog = await DialogService.GetDialogById(id);
            List<UserDTO> userList = await DialogService.GetUsersInDialog(dialog);

            UserDTO user = await UserService.GetUserById(User.Identity.GetUserId());
            if (userList.Find(x => x.Id == user.Id) == null)
                return RedirectToAction("AccessDenied", "Home");

            List<MessageDTO> list = (await MessageService
                .ListDialogMessages(new MessageFilter { Chat = dialog }, page))
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
        public async Task<ActionResult> OpenDialog(OpenDialogModel model)
        {
            Task<UserDTO> user = UserService.GetUserById(User.Identity.GetUserId());
            Task<DialogDTO> Dialog = DialogService.GetDialogById(model.DialogId);
            await Task.WhenAll(user, Dialog);
            await MessageService.PostMessageToDialog(Dialog.Result, user.Result, model.NewMessage);
            return RedirectToAction("OpenDialog", new { id = Dialog.Result.Id });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteMessage(MessageViewModel model)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            var message = await MessageService.GetMessageById(model.Id);
            await MessageService.DeleteMessage(message);
            return Redirect(Url);
        }

        [HttpPost]
        public async Task<ActionResult> EditMessage(MessageViewModel model)
        {
            string Url = Request.UrlReferrer.AbsolutePath;
            var message = await MessageService.GetMessageById(model.Id);
            message.Content = model.Content;
            await MessageService.EditMessage(message);
            return Redirect(Url);
        }

        public async Task<ActionResult> Edit(int id)
        {
            DialogDTO Dialog = await DialogService.GetDialogById(id);

            if (!(await DialogService.GetUsersInDialog(Dialog)).Any(x => x.Id == User.Identity.GetUserId())) return RedirectToAction("AccessDenied", "Home");

            return PartialView(Dialog);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(DialogDTO dialog)
        {
            await DialogService.EditDialogName(dialog);
            return RedirectToAction("OpenDialog", new { id = dialog.Id });
        }

        public async Task<ActionResult> RemovePeople(int id)
        {
            Task<DialogDTO> dialog = DialogService.GetDialogById(id);
            Task<UserDTO> user = UserService.GetUserById(User.Identity.GetUserId());
            await Task.WhenAll(dialog, user);
            List<UserDTO> userList = await DialogService.GetUsersInDialog(dialog.Result);
            if (!userList.Contains(user.Result)) return RedirectToAction("AccessDenied", "Home");
            userList.Remove(user.Result);
            List<SelectModel> list = new List<SelectModel>();
            userList.ForEach(f => list.Add(new SelectModel { User = f }));
            return View(new RemovePeopleModel { Accounts = list, Dialog = dialog.Result });
        }

        [HttpPost]
        public ActionResult RemovePeople(RemovePeopleModel model)
        {
            model.Accounts.Where(a => a.Invited).ForEach(a => DialogService.RemoveUserFromDialog(model.Dialog, a.User));

            return RedirectToAction("OpenDialog", "Dialog", new { id = model.Dialog.Id });
        }

        public async Task<ActionResult> Leave(int id)
        {
            DialogDTO Dialog = await DialogService.GetDialogById(id);
            if (!(await DialogService.GetUsersInDialog(Dialog)).Any(x => x.Id == User.Identity.GetUserId())) return RedirectToAction("AccessDenied", "Home");

            return PartialView(Dialog);
        }

        [HttpPost]
        public async Task<ActionResult> Leave(DialogDTO Dialog)
        {
            UserDTO user = await UserService.GetUserById(User.Identity.GetUserId());
            await DialogService.RemoveUserFromDialog(Dialog, user);
            return RedirectToAction("Index");
        }
    }
}
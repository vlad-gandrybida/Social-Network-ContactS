namespace ContactS.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);

        IMessageService CreateMessageService(string connection);

        IDialogService CreateDialogService(string connection);

        IRequestService CreateRequestService(string connection);
    }
}

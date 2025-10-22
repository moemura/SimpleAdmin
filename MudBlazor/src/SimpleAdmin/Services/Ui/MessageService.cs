using MudBlazor;

namespace SimpleAdmin.Services.Ui;

public interface IMessageService
{
    void Success(string message);
    void Error(string message);
    void Warning(string message);
    void Info(string message);
}

public class MessageService : IMessageService
{
    private readonly ISnackbar _snackbar;

    public MessageService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void Success(string message)
    {
        _snackbar.Add(message, Severity.Success);
    }

    public void Error(string message)
    {
        _snackbar.Add(message, Severity.Error);
    }

    public void Warning(string message)
    {
        _snackbar.Add(message, Severity.Warning);
    }

    public void Info(string message)
    {
        _snackbar.Add(message, Severity.Info);
    }
}

namespace HogOscChatTester.ViewModels;

/// <summary>
/// 
/// </summary>
public class MainWindowViewModel : WindowViewModelBase
{
    /// <summary>
    /// 
    /// </summary>
    public MainViewModel MainViewModel
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public IServer? Server
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public MainWindowViewModel()
    {
        this.Server = null;
        this.MainViewModel = new MainViewModel(this);

        this.Router.Navigate.Execute(this.MainViewModel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="server"></param>
    public MainWindowViewModel(IServer? server)
    {
        if (server is null)
        {
            throw new InvalidOperationException("Server did not get initialized on startup.");
        }

        this.Server = server;
        this.MainViewModel = new MainViewModel(this, this.Server);

        this.Router.Navigate.Execute(this.MainViewModel);
    }
}

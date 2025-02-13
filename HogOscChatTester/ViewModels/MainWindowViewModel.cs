namespace HogOscChatTester.ViewModels;

/// <summary>
/// The ViewModel for the <see cref="MainWindow"/> classe.
/// </summary>
public class MainWindowViewModel : WindowViewModelBase
{
    /// <summary>
    /// The <see cref="RoutableViewModelBase"/> meant for the
    /// Hog Chat OSC viewer.
    /// </summary>
    public MainViewModel MainViewModel
    {
        get;
    }

    /// <summary>
    /// The <see cref="OscServer"/> used to recieved any messages.
    /// </summary>
    public IServer? Server
    {
        get;
    }

    /// <summary>
    /// The primary constructor to be primarily used
    /// for the Avalonia XML Viewer.
    /// </summary>
    public MainWindowViewModel()
    {
        this.Server = new OscServer(new OscDispatcher());
        this.MainViewModel = new MainViewModel();

        this.Router.Navigate.Execute(this.MainViewModel);
    }

    /// <summary>
    /// The constructor that takes an <see cref="OscServer"/> meant
    /// to be passed down to other Views and their ViewModels.
    /// </summary>
    /// <param name="server">
    /// The <see cref="OscServer"/> created when the 
    /// application first starts.
    /// </param>
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

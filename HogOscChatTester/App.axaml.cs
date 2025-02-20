namespace HogOscChatTester;

public partial class App : Application
{
    /// <summary>
    /// The OSC Dispatcher service used to help 
    /// "dispatch" OSC addresses to the server.
    /// </summary>
    private readonly Models.Interfaces.IDispatcher _dispatcher = new OscDispatcher();

    /// <summary>
    /// The OSC Server used to recieve OSC 
    /// messages from a Client.
    /// </summary>
    private IServer? _server;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chatline1"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chatline2"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chatline3"));

        this._server = new OscServer(this._dispatcher);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(this._server)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

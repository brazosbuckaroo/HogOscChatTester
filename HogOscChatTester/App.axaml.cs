namespace HogOscChatTester;

public partial class App : Application
{
    /// <summary>
    /// The OSC Dispatcher service used to help 
    /// "dispatch" OSC addresses to the server.
    /// </summary>
    private Models.Interfaces.IDispatcher? _dispatcher;

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
        this._dispatcher = new OscDispatcher();

        this._dispatcher.AddAddress(new OscAddress("/hog/status/chatline1"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chatline2"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chatline3"));
        // related to a Hog defect. will remove once it's fixed.
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chat/line1"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chat/line2"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chat/line3"));

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

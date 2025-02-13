using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using HogOscChatTester.Models.Types;
using HogOscChatTester.Models.Interfaces;
using HogOscChatTester.ViewModels;
using HogOscChatTester.Views;
using OscCore.Address;

namespace HogOscChatTester;

/// <summary>
/// 
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// 
    /// </summary>
    private Models.Interfaces.IDispatcher? _dispatcher;

    /// <summary>
    /// 
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
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chat/line1"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chat/line2"));
        this._dispatcher.AddAddress(new OscAddress("/hog/status/chat/line3"));

        this._server = new OscServer(this._dispatcher);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

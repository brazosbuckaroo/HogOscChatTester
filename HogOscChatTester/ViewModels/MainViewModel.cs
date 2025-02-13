namespace HogOscChatTester.ViewModels;

/// <summary>
/// The ViewModel for the <see cref="MainView"/>. Holds all
/// the application logic.
/// </summary>
public class MainViewModel : RoutableViewModelBase
{
    /// <summary>
    /// This is where the ChatLine1 <see cref="OscAddress"/>
    /// message will be shown to the user.
    /// </summary>
    public string ChatLineOne
    {
        get => this._chatLineOne;
        set => this.RaiseAndSetIfChanged(ref this._chatLineOne, value);
    }

    /// <summary>
    /// This is where the ChatLine2 <see cref="OscAddress"/>
    /// message will be shown to the user.
    /// </summary>
    public string ChatLineTwo
    {
        get => this._chatLineTwo;
        set => this.RaiseAndSetIfChanged(ref this._chatLineTwo, value);
    }

    /// <summary>
    /// This is where the ChatLine3 <see cref="OscAddress"/>
    /// message will be shown to the user.
    /// </summary>
    public string ChatLineThree
    {
        get => this._chatLineThree;
        set => this.RaiseAndSetIfChanged(ref this._chatLineThree, value);
    }

    /// <summary>
    /// This is the backing property for user Port
    /// inout.
    /// </summary>
    public string? Port
    {
        get => this._port;
        set => this.RaiseAndSetIfChanged(ref this._port, value);
    }

    /// <summary>
    /// Used to track whether we have opened the 
    /// <see cref="UdpClient"/> port.
    /// </summary>
    public bool IsPortOpen
    {
        get => this._isPortOpen;
        set => this.RaiseAndSetIfChanged(ref this._isPortOpen, value);
    }

    /// <summary>
    /// The <see cref="OscServer"/> used to recieve an
    /// <see cref="OscMessage"/> using UDP.
    /// </summary>
    public IServer Server
    {
        get;
        set;
    }

    /// <summary>
    /// The <see cref="ReactiveCommand"/> used to signal 
    /// whether we want to open the port or close it.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ChangePortStatus
    {
        get;
    }

    /// <summary>
    /// Tracks the validity of user input. It will prevent the user
    /// from opening the port if they provide an invalid number.
    /// </summary>
    public IObservable<IValidationState> PortValidationState
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    private string _chatLineOne;

    /// <summary>
    /// 
    /// </summary>
    private string _chatLineTwo;

    /// <summary>
    /// 
    /// </summary>
    private string _chatLineThree;

    /// <summary>
    /// 
    /// </summary>
    private string? _port;

    /// <summary>
    /// 
    /// </summary>
    private bool _isPortOpen;

    /// <summary>
    /// 
    /// </summary>
    public MainViewModel()
    {
        this.ChangePortStatus = ReactiveCommand.Create(this.ChangePortStatusCommand,
                                                       this.CanOpenPort());
        this.ChatLineOne = string.Empty;
        this._chatLineOne = string.Empty;
        this.ChatLineTwo = string.Empty;
        this._chatLineTwo = string.Empty;
        this.ChatLineThree = string.Empty;
        this._chatLineThree = string.Empty;
        this.Port = "7001";
        this._port = "7001";
        this.IsPortOpen = false;
        this._isPortOpen = false;
        this.Server = new OscServer(new OscDispatcher());
        this.Server.OscMessageRecieved += this.Server_OscMessageRecieved!;
        this.PortValidationState = this.WhenValueChanged(thisViewModel => thisViewModel.Port)
                                       .Select(this.IsValidPortNumber);

        this.WhenActivated(disposables =>
        {
            this.ValidationRule(thisViewModel => thisViewModel.Port, this.PortValidationState)
                .DisposeWith(disposables);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    public MainViewModel(IScreen hostScreen, IServer server)
    {
        this.HostScreen = hostScreen;
        this.ChangePortStatus = ReactiveCommand.Create(this.ChangePortStatusCommand,
                                                       this.CanOpenPort());
        this.ChatLineOne = string.Empty;
        this._chatLineOne = string.Empty;
        this.ChatLineTwo = string.Empty;
        this._chatLineTwo = string.Empty;
        this.ChatLineThree = string.Empty;
        this._chatLineThree = string.Empty;
        this.Port = "7001";
        this._port = "7001";
        this.IsPortOpen = false;
        this._isPortOpen = false;
        this.Server = server;
        this.Server.OscMessageRecieved += this.Server_OscMessageRecieved!;
        this.PortValidationState = this.WhenValueChanged(thisViewModel => thisViewModel.Port)
                                       .Select(this.IsValidPortNumber);

        this.WhenActivated(disposables =>
        {
            this.ValidationRule(thisViewModel => thisViewModel.Port, this.PortValidationState)
                .DisposeWith(disposables);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IValidationState IsValidPortNumber(string? portNumber)
    {
        if (!int.TryParse(portNumber, out int parsedValue) || portNumber == null)
        {
            return new ValidationState(false, "Port must be a number");
        }
        if (parsedValue <= 0)
        {
            return new ValidationState(false, "Port must be a number greater than 0");
        }

        return ValidationState.Valid;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IObservable<bool> CanOpenPort()
    {
        return this.WhenAnyValue(thisViewModel => thisViewModel.Port,
                                 port => this.IsValidPortNumber(port).IsValid);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Server_OscMessageRecieved(object sender, OscMessageRecievedEventArgs e)
    {
        // any null values, no need to report them
        if (e.Message[0].ToString() == null)
        {
            return;
        }
        if (this.Server.Dispatcher.Addresses[0].Match(e.Message.Address)
            || this.Server.Dispatcher.Addresses[3].Match(e.Message.Address))
        {
            this.ChatLineOne = e.Message[0].ToString()!;
        }
        if (this.Server.Dispatcher.Addresses[1].Match(e.Message.Address)
            || this.Server.Dispatcher.Addresses[4].Match(e.Message.Address))
        {
            this.ChatLineTwo = e.Message[0].ToString()!;
        }
        if (this.Server.Dispatcher.Addresses[2].Match(e.Message.Address)
            || this.Server.Dispatcher.Addresses[5].Match(e.Message.Address))
        {
            this.ChatLineThree = e.Message[0].ToString()!;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void ChangePortStatusCommand()
    {
        if (!int.TryParse(this.Port, out int portNumber))
        {
            throw new InvalidOperationException("An invalid port number was given to the server.");
        }
        if (!this.IsPortOpen)
        {
            this.Server.EndConnection();
        }
        else
        {
            this.Server.BeginConnection(portNumber);
        }
    }
}

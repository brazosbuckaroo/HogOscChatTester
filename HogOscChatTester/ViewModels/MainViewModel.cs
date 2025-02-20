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
    /// Tracks the validity of user input. It will prevent the user
    /// from opening the port if they provide an invalid number.
    /// </summary>
    public IObservable<IValidationState> PortValidationState
    {
        get;
    }

    /// <summary>
    /// The list of IP addresses on the device.
    /// </summary>
    public ObservableCollection<string> HostDeivceIpAddresses
    {
        get => this._hostDeviceIpAddresses;
        set => this.RaiseAndSetIfChanged(ref this._hostDeviceIpAddresses, value);
    }

    /// <summary>
    /// The user selected IpV4 address to use when starting the server.
    /// </summary>
    public string SelectedIpAddress
    {
        get => this._selectedIpAddress;
        set => this.RaiseAndSetIfChanged(ref this._selectedIpAddress, value);
    }

    /// <summary>
    /// The <see cref="ReactiveCommand"/> used to signal 
    /// whether we want to open the port or close it.
    /// </summary>
    public ReactiveCommand<Unit, bool> ChangePortStatus
    {
        get;
    }

    /// <summary>
    /// The backing field for the <see cref="MainViewModel.ChatLineOne"> property
    /// to allow proper binding.
    /// </summary>
    private string _chatLineOne;

    /// <summary>
    /// The backing field for the <see cref="MainViewModel.ChatLineTwo"> property
    /// to allow proper binding.
    /// </summary>
    private string _chatLineTwo;

    /// <summary>
    /// The backing field for the <see cref="MainViewModel.ChatLineThree"> property
    /// to allow proper binding.
    /// </summary>
    private string _chatLineThree;

    /// <summary>
    /// The backing field for the <see cref="MainViewModel.Port"/> property
    /// to allow proper binding.
    /// </summary>
    private string? _port;

    /// <summary>
    /// The backing field for the <see cref="MainViewModel.IsPortOpen"/> property
    /// to allow proper binding.
    /// </summary>
    private bool _isPortOpen;

    /// <summary>
    /// The backing field for the list of IPAddresses on the 
    /// host device.
    /// </summary>
    private ObservableCollection<string> _hostDeviceIpAddresses;

    /// <summary>
    /// The IPV4 address selected by the user to open the 
    /// UDP port.
    /// </summary>
    private string _selectedIpAddress;

    /// <summary>
    /// A <see cref="IPHostEntry"/> classes to help
    /// us query the host machine for available addresses.
    /// </summary>
    private IPHostEntry _host;

    /// <summary>
    /// Once again, this is only for the Avalonia XML Previwer.
    /// </summary>
    public MainViewModel()
    {
        this.ChatLineOne = string.Empty;
        this._chatLineOne = string.Empty;
        this.ChatLineTwo = string.Empty;
        this._chatLineTwo = string.Empty;
        this.ChatLineThree = string.Empty;
        this._chatLineThree = string.Empty;
        this._host = Dns.GetHostEntry(Dns.GetHostName());
        this.HostDeivceIpAddresses = this.GetListOfIpAddresses();
        this._hostDeviceIpAddresses = this.GetListOfIpAddresses();
        this.SelectedIpAddress = string.Empty;
        this._selectedIpAddress = string.Empty;
        this.Port = "7001";
        this._port = "7001";
        this.IsPortOpen = false;
        this._isPortOpen = false;
        this.Server = new OscServer(new OscDispatcher());
        this.Server.OscMessageRecieved += this.Server_OscMessageRecieved!;
        this.PortValidationState = this.WhenValueChanged(thisViewModel => thisViewModel.Port)
                                       .Select(this.IsValidPortNumber);
        NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        this.ChangePortStatus = ReactiveCommand.CreateFromTask<bool>(this.ChangePortStatusCommand,
                                                                     this.CanOpenPort());

        this.WhenActivated(disposables =>
        {
            this.ValidationRule(thisViewModel => thisViewModel.Port, this.PortValidationState)
                .DisposeWith(disposables);
            this.Server.DisposeWith(disposables);
        });
    }

    /// <summary>
    /// The constructor that should be used as it assigns a 
    /// "hostScreen" to this ViewModel.
    /// </summary>
    /// <param name="hostScreen">
    /// The window that controls when this ViewModel is
    /// displayed.
    /// </param>
    /// <param name="server">
    /// The <see cref="OscServer"/> to be used for refreshing the UI
    /// with incoming <see cref="OscMessage"/>.
    /// </param>
    public MainViewModel(IScreen hostScreen, IServer server)
    {
        this.HostScreen = hostScreen;
        this.ChatLineOne = string.Empty;
        this._chatLineOne = string.Empty;
        this.ChatLineTwo = string.Empty;
        this._chatLineTwo = string.Empty;
        this.ChatLineThree = string.Empty;
        this._chatLineThree = string.Empty;
        this._host = Dns.GetHostEntry(Dns.GetHostName());
        this.HostDeivceIpAddresses = this.GetListOfIpAddresses();
        this._hostDeviceIpAddresses = this.GetListOfIpAddresses();
        this.SelectedIpAddress = string.Empty;
        this._selectedIpAddress = string.Empty;
        this.Port = "7001";
        this._port = "7001";
        this.IsPortOpen = false;
        this._isPortOpen = false;
        this.Server = server;
        this.Server.OscMessageRecieved += this.Server_OscMessageRecieved!;
        this.PortValidationState = this.WhenValueChanged(thisViewModel => thisViewModel.Port)
                                       .Select(this.IsValidPortNumber);
        NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        this.ChangePortStatus = ReactiveCommand.CreateFromTask<bool>(this.ChangePortStatusCommand,
                                                                     this.CanOpenPort());

        this.WhenActivated(disposables =>
        {
            this.ValidationRule(thisViewModel => thisViewModel.Port, this.PortValidationState)
                .DisposeWith(disposables);
            this.Server.DisposeWith(disposables);
        });
    }

    /// <summary>
    /// The function that actually validates user input.
    /// </summary>
    /// <param name="portNumber">
    /// A string input to be parsed and checked for 
    /// validity.
    /// </param>
    /// <returns>
    /// A <see cref="IValidationState"/> that is used to 
    /// inform the UI if the user provided a valid port
    /// number.
    /// </returns>
    private IValidationState IsValidPortNumber(string? inputPortNumber)
    {
        if (!int.TryParse(inputPortNumber, out int parsedValue) || inputPortNumber == null)
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
    /// A helper function to check if the provided IPV4 address
    /// is valid before letting it be used to open the port.
    /// </summary>
    /// <param name="inputIpAddress">
    /// An IPAddress provided as a string.
    /// </param>
    /// <returns>
    /// Returns a <see cref="IValidationState"/> that can be
    /// valid or not based on the Ip Address given.
    /// </returns>
    private IValidationState IsAvailableIpAddress(string? inputIpAddress)
    {
        if (inputIpAddress == null || !IPAddress.TryParse(inputIpAddress, out IPAddress? parsedAddress))
        {
            return new ValidationState(false, "Not a valid Ip Address.");
        }
        if (parsedAddress == null)
        {
            return new ValidationState(false, "Could not parse inputIpAddress");
        }

        foreach (IPAddress ipAddress in this._host.AddressList)
        {
            // check to make sure we still have this ip address
            if (ipAddress.Equals(parsedAddress))
            {
                return ValidationState.Valid;
            }
        }

        return new ValidationState(false, "Not an available Ip Address.");
    }

    /// <summary>
    /// Everytime <see cref="MainViewModel.Port"/> is updated in the UI, 
    /// this method will check the input and return whether it was valid.
    /// </summary>
    /// <returns>'
    /// An <see cref="IObservable{T}"/> used to track the validaity of the user
    /// provided port number everytime there is an update to the <see cref="MainView"/>.
    /// </returns>
    private IObservable<bool> CanOpenPort()
    {
        return this.WhenAnyValue(thisViewModel => thisViewModel.Port,
                                 thisViewModel => thisViewModel.SelectedIpAddress,
                                 (port, selectedIpAddress) => this.IsValidPortNumber(port).IsValid 
                                                              && !string.IsNullOrEmpty(selectedIpAddress)
                                                              && this.IsAvailableIpAddress(selectedIpAddress).IsValid);
    }

    /// <summary>
    /// The actual funationality of the <see cref="MainViewModel.ChangePortStatus"/>
    /// command.
    /// </summary>
    /// <returns>
    /// Returns a bool to signal when to disable and enable user input.
    /// </returns>
    private async Task<bool> ChangePortStatusCommand()
    {
        // we should never get here; hence, the reason I want to crash
        // if we manage to get here, then we have bigger issues :P
        if (!int.TryParse(this.Port, out int portNumber))
        {
            throw new InvalidOperationException("An invalid port number was given to the server.");
        }
        if (this.IsPortOpen)
        {
            this.Server.BeginConnection(this.SelectedIpAddress, portNumber);

            return true;
        }
        else
        {
            await this.Server.EndConnection();

            return false;
        }
    }

    /// <summary>
    /// Get a list of IPV4 address on the local
    /// machine.
    /// </summary>
    /// <returns>
    /// Returns a list in the form of an <see cref="ObservableCollection"/>.
    /// </returns>
    private ObservableCollection<string> GetListOfIpAddresses()
    {
        ObservableCollection<string> listOfAddress = new ObservableCollection<string>();

        // TODO: Make this async perhaps?!?!?
        foreach (IPAddress address in this._host.AddressList)
        {
            // check for IPV4 addresses only
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                listOfAddress.Add(address.ToString());
            }
        }

        return listOfAddress;
    }

    /// <summary>
    /// Everytime the <see cref="OscServer"/> send a "I have recieved a message" signal, this 
    /// Event Handler will check the input to determine how and where it would shown to the user.
    /// </summary>
    /// <param name="sender">
    /// The server that sends the signal that there was an <see cref="OscMessage"/>.
    /// </param>
    /// <param name="e">
    /// The signal that contains the <see cref="OscMessage"/> from the server.
    /// </param>
    private void Server_OscMessageRecieved(object sender, OscMessageRecievedEventArgs e)
    {
        // any null values, no need to report them
        if (e.Message[0].ToString() == null)
        {
            return;
        }
        if (this.Server.Dispatcher.Addresses[0].Match(e.Message.Address))
        {
            this.ChatLineOne = e.Message[0].ToString()!;
        }
        if (this.Server.Dispatcher.Addresses[1].Match(e.Message.Address))
        {
            this.ChatLineTwo = e.Message[0].ToString()!;
        }
        if (this.Server.Dispatcher.Addresses[2].Match(e.Message.Address))
        {
            this.ChatLineThree = e.Message[0].ToString()!;
        }
    }

    /// <summary>
    /// An event handler used to listen to any Network Adapter
    /// changes on the system.
    /// </summary>
    /// <param name="sender">
    /// The <see cref="NetworkChange"/> that sends the message we have new
    /// ip addresses
    /// </param>
    /// <param name="e">
    /// For this event handler this can be safely ignored.
    /// </param>
    private async void NetworkChange_NetworkAddressChanged(object? sender, EventArgs e)
    {
        // check to see if the port is opened
        // when the network address changes
        // then, well close it. 
        if (this.IsPortOpen)
        {
            this.IsPortOpen = false;

            await this.ChangePortStatus.Execute();
        }

        // refresh list
        this._host = Dns.GetHostEntry(Dns.GetHostName());
        // refresh UI
        this.HostDeivceIpAddresses = this.GetListOfIpAddresses();
    }
}

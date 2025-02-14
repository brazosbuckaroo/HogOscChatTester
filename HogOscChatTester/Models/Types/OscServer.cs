namespace HogOscChatTester.Models.Types;

/// <summary>
/// A simple <see cref="OscServer"/> used to recieve 
/// <see cref="OscMessage"/>.
/// </summary>
public class OscServer : Models.Interfaces.IServer
{
    /// <inheritdoc/>
    public IPAddress IpAddress
    {
        get;
    }

    /// <inheritdoc/>
    public Models.Interfaces.IDispatcher Dispatcher
    {
        get;
    }

    /// <inheritdoc/>
    public UdpClient? UdpClient
    {
        get;
        private set;
    }

    /// <inheritdoc/>
    public event EventHandler<OscMessageRecievedEventArgs>? OscMessageRecieved;

    /// <summary>
    /// The cancellation token used to cancel
    /// the recieve messages task.
    /// </summary>
    private CancellationToken _serverTaskCancellation;

    /// <summary>
    /// The task used to control and communicate what the 
    /// listening thread should do. (i.e. cancelling, etc)
    /// </summary>
    private Task? _serverTask;

    /// <summary>
    /// A default constructor that will allow the server
    /// to listen to any messages over any network device.
    /// </summary>
    public OscServer()
    {
        this.IpAddress = IPAddress.Any;
        this.UdpClient = null;
        this.Dispatcher = new OscDispatcher();
        this._serverTask = null;
        this._serverTaskCancellation = default;
    }

    /// <summary>
    /// The constructor that will allow OSC dispatching and
    /// and listening to traffic over any address.
    /// </summary>
    /// <param name="dispatcher">
    /// The <see cref="OscDispatcher"/> used to help dispatch messages as 
    /// needed.
    /// </param>
    public OscServer(Models.Interfaces.IDispatcher dispatcher)
    {
        this.IpAddress = IPAddress.Any;
        this.UdpClient = null;
        this.Dispatcher = dispatcher;
        this._serverTask = null;
        this._serverTaskCancellation = default;
    }

    /// <summary>
    /// The constructor that allows a specific <see cref="IPAddress"/>
    /// to be used as an endpoint with dispatching.
    /// </summary>
    /// <param name="address">
    /// The network deivces Ip address.
    /// </param>
    /// <param name="dispatcher">
    /// The dispatcher that'll allow <see cref="OscMessage"/> to be 
    /// filtered.
    /// </param>
    public OscServer(IPAddress address, Models.Interfaces.IDispatcher dispatcher)
    {
        this.IpAddress = address;
        this.UdpClient = null;
        this.Dispatcher = dispatcher;
        this._serverTask = null;
        this._serverTaskCancellation = default;
    }

    /// <summary>
    /// The method to allow listening to incoming <see cref="OscMessage"/>
    /// </summary>
    /// <param name="cancellation">
    /// The <see cref="CancellationToken"/> used to request a cancellation.
    /// </param>
    /// <returns>
    /// Returns a <see cref="Task"/> to run the <see cref="UdpClient"/>
    /// </returns>
    private async Task ListenTaskAsync(CancellationToken cancellation = default)
    {
        while (this.UdpClient is not null && cancellation == CancellationToken.None)
        {
            try
            {
                UdpReceiveResult datagram = await this.UdpClient.ReceiveAsync(cancellation);
                OscMessage message = OscMessage.Read(datagram.Buffer, 0, datagram.Buffer.Length);

                if (this.Dispatcher.IsExpectedAddress(message.Address))
                {
                    OscMessageRecievedEventArgs args = new OscMessageRecievedEventArgs(message);

                    this.OnOscMessageRecieved(args);
                }
            }
            catch (SocketException)
            {
                break;
            }
        }
    }

    /// <inheritdoc/>
    public void BeginConnection(int port)
    {
        this.UdpClient = new UdpClient(port);
        this._serverTaskCancellation = CancellationToken.None;
        this._serverTask = Task.Run(async () => 
        { 
            await this.ListenTaskAsync(this._serverTaskCancellation); 
        }, this._serverTaskCancellation);
    }

    /// <inheritdoc/>
    public void EndConnection()
    {
        if (this.UdpClient == null)
        {
            return;
        }

        var tokenSource = new CancellationTokenSource();
        this._serverTaskCancellation = tokenSource.Token;

        tokenSource.Cancel();
        this.UdpClient.Close();
        tokenSource.Dispose();

        this.UdpClient = null;
    }

    /// <summary>
    /// The event handler used to signal the listener
    /// that an <see cref="OscMessage"/> has been recieved.
    /// </summary>
    /// <param name="e">
    /// The event argument used to give the <see cref="OscMessage"/>
    /// to the caller.
    /// </param>
    protected virtual void OnOscMessageRecieved(OscMessageRecievedEventArgs e)
    {
        this.OscMessageRecieved?.Invoke(this, e);
    }
}

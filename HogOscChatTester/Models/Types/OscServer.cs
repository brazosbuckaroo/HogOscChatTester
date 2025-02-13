namespace HogOscChatTester.Models.Types;

/// <summary>
/// 
/// </summary>
public class OscServer : Models.Interfaces.IServer
{
    /// <inheritdoc/>
    public IPAddress IpAddress
    {
        get;
    }

    /// <inheritdoc/>
    public Interfaces.IDispatcher Dispatcher
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
    /// 
    /// </summary>
    private CancellationToken _serverTaskCancellation;

    /// <summary>
    /// 
    /// </summary>
    private Task? _serverTask;

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    /// <param name="dispatcher"></param>
    public OscServer(Models.Interfaces.IDispatcher dispatcher)
    {
        this.IpAddress = IPAddress.Any;
        this.UdpClient = null;
        this.Dispatcher = dispatcher;
        this._serverTask = null;
        this._serverTaskCancellation = default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="dispatcher"></param>
    public OscServer(IPAddress address, Models.Interfaces.IDispatcher dispatcher)
    {
        this.IpAddress = address;
        this.UdpClient = null;
        this.Dispatcher = dispatcher;
        this._serverTask = null;
        this._serverTaskCancellation = default;
    }

    /// <inheritdoc/>
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
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnOscMessageRecieved(OscMessageRecievedEventArgs e)
    {
        this.OscMessageRecieved?.Invoke(this, e);
    }
}

using Avalonia.Input;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;

namespace HogOscChatTester.Models.Types;

/// <summary>
/// A simple <see cref="OscServer"/> used to recieve 
/// <see cref="OscMessage"/>.
/// </summary>
public class OscServer : Models.Interfaces.IServer, IDisposable
{
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
    /// 
    /// </summary>
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// The task used to control and communicate what the 
    /// listening thread should do. (i.e. cancelling, etc)
    /// </summary>
    private Task? _serverTask;

    /// <summary>
    /// 
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// A default constructor that will allow the server
    /// to listen to any messages over any network device.
    /// </summary>
    public OscServer()
    {
        this.UdpClient = null;
        this.Dispatcher = new OscDispatcher();
        this._serverTask = null;
        this._cancellationTokenSource = null;
        this._serverTaskCancellation = default;
        this._isDisposed = false;
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
        this.UdpClient = null;
        this.Dispatcher = dispatcher;
        this._serverTask = null;
        this._cancellationTokenSource = null;
        this._serverTaskCancellation = default;
        this._isDisposed = false;
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
        while (this.UdpClient != null && !cancellation.IsCancellationRequested)
        {
            UdpReceiveResult datagram = await this.UdpClient.ReceiveAsync(cancellation);
            OscMessage message = OscMessage.Read(datagram.Buffer, 0, datagram.Buffer.Length);

            if (this.Dispatcher.IsExpectedAddress(message.Address))
            {
                OscMessageRecievedEventArgs args = new OscMessageRecievedEventArgs(message);

                this.OnOscMessageRecieved(args);
            }
        }
    }

    /// <inheritdoc/>
    public void BeginConnection(string ipAddress, int port)
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        this.UdpClient = new UdpClient(localEndPoint);
        this._cancellationTokenSource = new CancellationTokenSource();
        this._serverTaskCancellation = this._cancellationTokenSource.Token;
        this._serverTask = Task.Run(async () => 
        { 
            await this.ListenTaskAsync(this._serverTaskCancellation); 
        }, this._serverTaskCancellation);
    }

    /// <inheritdoc/>
    public async Task EndConnection(CancellationToken cancellation = default)
    {
        if (this.UdpClient == null 
            || this._serverTask == null
            || this._cancellationTokenSource == null)
        {
            return;
        }

        try
        {
            await this._cancellationTokenSource.CancelAsync();
            this._serverTask.Wait(CancellationToken.None);
        }
        catch (AggregateException)
        {
            if (this._serverTask.IsCanceled || this._serverTask.IsFaulted)
            {
                this.UdpClient.Close();
                this._serverTask.Dispose();
                this._cancellationTokenSource.Dispose();

                this.UdpClient = null;
                this._serverTask = null;
                this._cancellationTokenSource = null;
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// The implentation of disposing for this class.
    /// </summary>
    /// <param name="disposing">
    /// Determine if we want to actuall fully dispose of 
    /// the <see cref="OscServer"/>.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (this._isDisposed)
        {
            return;
        }
        if (disposing)
        {
            if (this._serverTask != null)
            {
                // since we have to wait for 
                // the server to be shutdown before we dispose
                Task.Run(async () =>
                {
                    await this.EndConnection();
                }).Wait();
            }
        }

        this._isDisposed = true;
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

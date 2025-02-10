using Avalonia.Rendering;
using OscCore;
using OscCore.Address;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HogOscChatTester.Models.Types;

/// <summary>
/// 
/// </summary>
public class OscServer
{
    /// <summary>
    /// 
    /// </summary>
    public IPAddress Address 
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public int Port
    {
        get;
        private set;
    }

    /// <summary>
    /// 
    /// </summary>
    public OscDispatcher Dispatcher
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public UdpClient? UdpClient
    {
        get;
        private set;
    }

    /// <summary>
    /// 
    /// </summary>
    public Task? ServerTask
    {
        get; 
        private set;
    }

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<OscMessageRecievedEventArgs>? OscMessageRecieved;

    /// <summary>
    /// 
    /// </summary>
    private CancellationToken _serverTaskCancellation;

    /// <summary>
    /// 
    /// </summary>
    public OscServer()
    {
        this.Address = IPAddress.Any;
        this.Port = 7001;
        this.UdpClient = null;
        this.Dispatcher = new OscDispatcher();
        this.ServerTask = null;
        this._serverTaskCancellation = default;

        this.Dispatcher.AddAddress(new OscAddress("/hog/status/chatline1"));
        this.Dispatcher.AddAddress(new OscAddress("/hog/status/chatline2"));
        this.Dispatcher.AddAddress(new OscAddress("/hog/status/chatline3"));
    }

    /// <summary>
    /// 
    /// </summary>
    public OscServer(int port)
    {
        this.Address = IPAddress.Any;
        this.Port = port;
        this.UdpClient = null;
        this.Dispatcher = new OscDispatcher();
        this.ServerTask = null;
        this._serverTaskCancellation = default;

        this.Dispatcher.AddAddress(new OscAddress("/hog/status/chatline1"));
        this.Dispatcher.AddAddress(new OscAddress("/hog/status/chatline2"));
        this.Dispatcher.AddAddress(new OscAddress("/hog/status/chatline3"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="port"></param>
    /// <param name="dispatcher"></param>
    public OscServer(IPAddress address, int port, OscDispatcher dispatcher)
    {
        this.Address = address;
        this.Port = port;
        this.UdpClient = null;
        this.Dispatcher = dispatcher;
        this.ServerTask = null;
        this._serverTaskCancellation = default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    public void BeginConnection(int port)
    {
        this.Port = port;
        this.UdpClient = new UdpClient(this.Port);
        this._serverTaskCancellation = CancellationToken.None;
        this.ServerTask = Task.Run(async () => { await this.ListenTaskAsync(this._serverTaskCancellation); }, this._serverTaskCancellation);
    }

    /// <summary>
    /// 
    /// </summary>
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

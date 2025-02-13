using HogOscChatTester.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HogOscChatTester.Models.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IServer
{
    /// <summary>
    /// 
    /// </summary>
    UdpClient? UdpClient
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    IPAddress IpAddress
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    Interfaces.IDispatcher Dispatcher
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    event EventHandler<OscMessageRecievedEventArgs>? OscMessageRecieved;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="port"></param>
    void BeginConnection(int port);

    /// <summary>
    /// 
    /// </summary>
    void EndConnection();
}

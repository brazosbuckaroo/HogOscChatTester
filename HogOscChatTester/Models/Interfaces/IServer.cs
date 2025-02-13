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
    Models.Interfaces.IDispatcher Dispatcher
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

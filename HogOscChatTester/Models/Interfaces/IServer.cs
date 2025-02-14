namespace HogOscChatTester.Models.Interfaces;

/// <summary>
/// The interface used for the basis of the
/// <see cref="OscServer"/>.
/// </summary>
public interface IServer : IDisposable
{
    /// <summary>
    /// The <see cref="UdpClient"/> used to open the
    /// UDP port.
    /// </summary>
    UdpClient? UdpClient
    {
        get;
    }

    /// <summary>
    /// The IP address of the host machine.
    /// </summary>
    IPAddress IpAddress
    {
        get;
    }

    /// <summary>
    /// The dispatcher used to filter, or dispatch, 
    /// <see cref="OscMessage"/> for this server.
    /// </summary>
    Models.Interfaces.IDispatcher Dispatcher
    {
        get;
    }

    /// <summary>
    /// The eventhandler used to signal when 
    /// an <see cref="OscMessage"/> has been recieved.
    /// </summary>
    event EventHandler<OscMessageRecievedEventArgs>? OscMessageRecieved;

    /// <summary>
    /// The method used to open the UDP port so we 
    /// can listen for <see cref="OscMessage"/>
    /// being sent.
    /// </summary>
    /// <param name="port">
    /// The UDP port number to open so we
    /// can listen for an <see cref="OscMessage"/>.
    /// </param>
    void BeginConnection(int port);

    /// <summary>
    /// The method that will end the connection and stop reading 
    /// <see cref="OscMessage"/>.
    /// </summary>
    /// <param name="cancellation">
    /// A cancellation token to be passed down if needed.
    /// </param>
    /// <returns>
    /// Returns a task that can be awaited.
    /// </returns>
    Task EndConnection(CancellationToken cancellation = default);
}

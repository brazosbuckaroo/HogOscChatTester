namespace HogOscChatTester.Models.Interfaces;

/// <summary>
/// The dispatcher used to handle how <see cref="OscMessage"/>
/// are handled by the server.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// The list of address that need to be
    /// dispatcher to the server.
    /// </summary>
    List<OscAddress> Addresses
    {
        get;
    }

    /// <summary>
    /// A method that allows an <see cref="OscAddress"/> to 
    /// be added to the dispatcher.
    /// </summary>
    /// <param name="address">
    /// The new <see cref="OscAddress"/> to be added to the list
    /// </param>
    void AddAddress(OscAddress address);

    /// <summary>
    /// Compares the incoming messages with the 
    /// list of accepted <see cref="OscAddress"/>
    /// </summary>
    /// <param name="inputAddress">
    /// The address from the incoming <see cref="OscMessage"/>
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> used to verify if
    /// an <see cref="OscMessage"/> came from a wanted address.
    /// </returns>
    bool IsExpectedAddress(string inputAddress);
}

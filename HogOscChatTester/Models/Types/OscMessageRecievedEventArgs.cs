namespace HogOscChatTester.Models.Types;

/// <summary>
/// A simple eventhandler argument reciever used to 
/// send the <see cref="OscMessage"/> the method invoking
/// the eventhandler.
/// </summary>
/// <param name="message">
/// The latest <see cref="OscMessage"/> recieved by the
/// server.
/// </param>
public class OscMessageRecievedEventArgs(OscMessage message) : EventArgs
{
    /// <summary>
    /// The <see cref="OscMessage"/> recieved by the <see cref="OscServer"/>.
    /// </summary>
    public OscMessage Message
    {
        get;
    } = message;
}

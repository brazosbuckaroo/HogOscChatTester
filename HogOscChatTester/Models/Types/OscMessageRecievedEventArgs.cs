namespace HogOscChatTester.Models.Types;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public class OscMessageRecievedEventArgs(OscMessage message) : EventArgs
{
    /// <summary>
    /// 
    /// </summary>
    public OscMessage Message
    {
        get;
    } = message;
}

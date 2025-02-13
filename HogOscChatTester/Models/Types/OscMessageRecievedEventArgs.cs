using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

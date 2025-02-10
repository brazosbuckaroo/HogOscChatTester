using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogOscChatTester.Models.Types;

public class OscMessageRecievedEventArgs(OscMessage message) : EventArgs
{
    public OscMessage Message
    {
        get => message;
    }
}

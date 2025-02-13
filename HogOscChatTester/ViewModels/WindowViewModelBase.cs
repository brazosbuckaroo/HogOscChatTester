using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogOscChatTester.ViewModels;

/// <summary>
/// 
/// </summary>
public class WindowViewModelBase : ReactiveObject, IScreen
{
    /// <summary>
    /// 
    /// </summary>
    public RoutingState Router
    {
        get;
    } = new RoutingState();
}

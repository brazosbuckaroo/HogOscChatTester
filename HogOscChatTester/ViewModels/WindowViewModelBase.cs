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

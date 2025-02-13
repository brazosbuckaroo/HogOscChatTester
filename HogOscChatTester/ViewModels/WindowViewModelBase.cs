namespace HogOscChatTester.ViewModels;

/// <summary>
/// The base class for any Window the application
/// needs to make. Mostly used to allow for view 
/// routing purposes.
/// </summary>
public class WindowViewModelBase : ReactiveObject, IScreen
{
    /// <inheritdoc/>
    public RoutingState Router
    {
        get;
    } = new RoutingState();
}

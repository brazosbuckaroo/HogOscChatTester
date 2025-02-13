namespace HogOscChatTester.ViewModels;

/// <summary>
/// 
/// </summary>
public class RoutableViewModelBase : ReactiveObject, IRoutableViewModel, IValidatableViewModel, IActivatableViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public IScreen HostScreen
    {
        get;
        protected set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string? UrlPathSegment
    {
        get;
    } = Guid.NewGuid().ToString().Substring(0, 5);

    /// <summary>
    /// 
    /// </summary>
    public IValidationContext ValidationContext
    {
        get; 
    } = new ValidationContext();

    /// <summary>
    /// 
    /// </summary>
    public ViewModelActivator Activator
    {
        get; 
    } = new ViewModelActivator();
}

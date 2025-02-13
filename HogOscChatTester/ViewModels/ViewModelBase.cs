namespace HogOscChatTester.ViewModels;

/// <summary>
/// 
/// </summary>
public class ViewModelBase : ReactiveObject, IActivatableViewModel, IValidatableViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public ViewModelActivator Activator
    {
        get;
    } = new ViewModelActivator();

    /// <summary>
    /// 
    /// </summary>
    public IValidationContext ValidationContext
    {
        get;
    } = new ValidationContext();
}

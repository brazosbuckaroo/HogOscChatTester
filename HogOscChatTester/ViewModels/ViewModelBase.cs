namespace HogOscChatTester.ViewModels;

/// <summary>
/// A simple ViewModel base that allows for
/// properties to be disposed of and allows for 
/// validation of user input.
/// </summary>
public class ViewModelBase : ReactiveObject, IActivatableViewModel, IValidatableViewModel
{
    /// <inheritdoc/>
    public ViewModelActivator Activator
    {
        get;
    } = new ViewModelActivator();

    /// <inheritdoc/>
    public IValidationContext ValidationContext
    {
        get;
    } = new ValidationContext();
}

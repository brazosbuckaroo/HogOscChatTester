namespace HogOscChatTester.ViewModels;

/// <summary>
/// A routable ViewModel that will allow
/// a <see cref="WindowViewModelBase"/> to create a View
/// with it's correct ViewModel.
/// </summary>
public class RoutableViewModelBase : ReactiveObject, IRoutableViewModel, IValidatableViewModel, IActivatableViewModel
{
    /// <inheritdoc/>
    public IScreen HostScreen
    {
        get;
        protected set;
    }

    /// <inheritdoc/>
    public string? UrlPathSegment
    {
        get;
    } = Guid.NewGuid().ToString().Substring(0, 5);

    /// <inheritdoc/>
    public IValidationContext ValidationContext
    {
        get; 
    } = new ValidationContext();

    /// <inheritdoc/>

    public ViewModelActivator Activator
    {
        get; 
    } = new ViewModelActivator();
}

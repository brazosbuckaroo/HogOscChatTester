namespace HogOscChatTester.Models.Types;

/// <summary>
/// A simple class used to help assign Views
/// to their respective ViewModels.
/// </summary>
public class AppViewLocator : IViewLocator
{
    /// <inheritdoc/>
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        MainViewModel context => new MainView { DataContext = context },
        _ => throw new NotImplementedException()
    };
}

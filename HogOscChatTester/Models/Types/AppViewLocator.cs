namespace HogOscChatTester.Models.Types;

/// <inheritdoc/>
public class AppViewLocator : IViewLocator
{
    /// <inheritdoc/>
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        MainViewModel context => new MainView { DataContext = context },
        _ => throw new NotImplementedException()
    };
}

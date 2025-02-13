namespace HogOscChatTester.Views;

/// <summary>
/// The MainWindow class.
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    /// <summary>
    /// Default constructor to allow view routing and 
    /// dialog boxes to open.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.WhenActivated(disposable =>
        {
        });
    }
}

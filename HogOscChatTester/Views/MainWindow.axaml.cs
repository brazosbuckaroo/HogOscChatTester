namespace HogOscChatTester.Views;

/// <summary>
/// 
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    /// <summary>
    /// 
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.WhenActivated(disposable =>
        {
        });
    }
}

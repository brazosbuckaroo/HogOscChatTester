namespace HogOscChatTester.Views;

/// <summary>
/// The MainView class. Controls any UI changes.
/// </summary>
public partial class MainView : ReactiveUserControl<MainViewModel>
{
    /// <summary>
    /// Default constructor that will bind <see cref="MainViewModel"/>
    /// validation context to output and listen to <see cref="MainViewModel.PortValidationState"/>.
    /// </summary>
    public MainView()
    {
        this.InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.BindValidation(this.ViewModel, thisView => thisView.ValidationTextTip.Text)
                .DisposeWith(disposables);
            this.ViewModel!.PortValidationState.Subscribe(this.UpdatePathIcon)
                .DisposeWith(disposables);
        });
    }

    /// <summary>
    /// A method that checks the <see cref="MainViewModel.Port"/> 
    /// validity and changes the UI respective of the outcome.
    /// </summary>
    /// <param name="canOpenPort">
    /// The current state of <see cref="MainViewModel.PortValidationState"/>.
    /// </param>
    private void UpdatePathIcon(IValidationState? canOpenPort)
    {
        object? pathIconGeometry = null;

        this.PortValidationIcon.Classes.Clear();

        if (App.Current is null)
        {
            throw new InvalidProgramException("Application is null and cannot be used in UpdatePathIcon");
        }
        if (canOpenPort == null)
        {
            throw new InvalidProgramException("Invalid validationstate in UpdatePathIcon");
        }
        if (!canOpenPort.IsValid)
        {
            App.Current.TryGetResource("ErrorIconData", out pathIconGeometry);

            this.PortValidationIcon.Data = (StreamGeometry?)pathIconGeometry;
            this.PortValidationIcon.Classes.Set("Error", true);
            
            return;
        }

        App.Current.TryGetResource("OkayIconData", out pathIconGeometry);

        this.PortValidationIcon.Data = (Geometry?)pathIconGeometry;
        this.ValidationTextTip.Text = "Valid Port.";

        this.PortValidationIcon.Classes.Set("NoError", true);
    }
}

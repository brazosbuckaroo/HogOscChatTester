using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using HogOscChatTester.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.States;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;

namespace HogOscChatTester.Views;

/// <summary>
/// 
/// </summary>
public partial class MainView : ReactiveUserControl<MainViewModel>
{
    /// <summary>
    /// 
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
    /// 
    /// </summary>
    /// <param name="canOpenPort"></param>
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

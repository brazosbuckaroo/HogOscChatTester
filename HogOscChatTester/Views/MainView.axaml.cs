using Avalonia.Controls;
using Avalonia.ReactiveUI;
using HogOscChatTester.ViewModels;
using ReactiveUI;

namespace HogOscChatTester.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        this.InitializeComponent();

        this.WhenActivated(disposables =>
        {
        });
    }
}
